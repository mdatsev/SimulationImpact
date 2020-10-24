using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Simulations;
using System;
using System.Xml;

public class Visualization : MonoBehaviour
{
    public GameObject SimulationManager;
    public bool useDummySim;
    public List<Vector2> startingPoints = new List<Vector2>();
    public GameObject street;
    public GameObject building;
    public GameObject sidewalk;
    public GameObject traficL;
    public Map map = new Map();
    public float decorationChance = 0.0f;
    public float buildingChance = 0.3f;

    private Simulation sim;
    private List<Car> cars = new List<Car>();
    private GameObject streetWire;

    // Start is called before the first frame update
    void Start()
    {
        streetWire = new GameObject("StreetWire");
        GameObject streets = new GameObject("Streets");
        GameObject sidewalks = new GameObject("Sidewalks");
        GameObject traficLights = new GameObject("Sidewalks");
        GameObject decorations = new GameObject("Decorations");
        GameObject buildings = new GameObject("Buildings");
        streets.transform.parent = streetWire.transform;
        sidewalks.transform.parent = streetWire.transform;
        decorations.transform.parent = streetWire.transform;
        traficLights.transform.parent = streetWire.transform;
        buildings.transform.parent = streetWire.transform;

        reader();
        List<Edge> edg = map.edges;

        //Node n1 = new Node(new Vector3(5, 0, 0), true);
        //Node n2 = new Node(new Vector3(10, 0, 10), true);

        //edg.Add(new Edge(n1, n2, 1, 1, 60, ""));
        Debug.Log(edg.Count);

        GameObject[] decorationListArray = Resources.LoadAll<GameObject>("Prefabs/Decorations");
        List<GameObject> decorationList = decorationListArray.ToList();
        System.Random rand = new System.Random();

        GameObject[] buildingListArray = Resources.LoadAll<GameObject>("Prefabs/Buildings");
        List<GameObject> buildingList = buildingListArray.ToList();

        foreach (Edge e in edg)
        {
            int prefsNum = (int)Math.Ceiling(e.length / 4);

            for (int i=0; i <= prefsNum; i++)
            {
                float x = e.getStart().position.x + (e.direction.x)*((float)i / prefsNum);
                float z = e.getStart().position.z + (e.direction.z)*((float)i / prefsNum);
                Quaternion rotation = Quaternion.AngleAxis( -90 + (float)Math.Atan2((e.direction.x), (e.direction.z))*(180F/(float)Math.PI), Vector3.up);
                Vector3 pos = new Vector3(x, 0, z);
                Vector3 normal = Vector3.Cross(e.direction, new Vector3(0,1,0)).normalized;

                Instantiate(street, pos, rotation, streets.transform);
                
                if(e.getStart().traficLight && i ==0) {
                    Instantiate(traficL, pos + normal * 3, rotation, traficLights.transform);
                }

                if(e.getEnd().traficLight && i == prefsNum) {
                    Instantiate(traficL, pos + normal * 3, rotation, traficLights.transform);
                }
                
                if (rand.NextDouble() < buildingChance && i > 10)
                {
                    Instantiate(buildingList[UnityEngine.Random.Range(0, buildingList.Count)]
                        , pos + normal * 8, rotation, buildings.transform);
                }

                Instantiate(sidewalk, pos + normal * 3, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos + normal * 3, rotation, decorations.transform);
                }

                rotation *= Quaternion.Euler(0, 180, 0);
    
                Instantiate(sidewalk, pos - normal * 3, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos - normal * 3, rotation, decorations.transform);
                }
                
                if (rand.NextDouble() < buildingChance && i < prefsNum + 10)
                {
                    Instantiate(buildingList[UnityEngine.Random.Range(0, buildingList.Count)]
                        , pos - normal * 8, rotation, buildings.transform);
                }
            }
        }
    
        if (useDummySim) {
            sim = new SimulationDummy();
        } else {
            sim = new SimulationImpact();
        }

        //startingPoints.Add(new Vector2(0,0));
        //startingPoints.Add(new Vector2(1,1));

        GameObject[] carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        List<GameObject> carList = carListArray.ToList();
        // foreach (Vector2 p in startingPoints) {
        //     GameObject car = Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], new Vector3(p.x, 0, p.y), Quaternion.identity);
        //     Car c = car.GetComponent<Car>();
        //     c.changeRoad(new Edge(new Node(new Vector3(0,0,0)), new Node(new Vector3(1,0,1)), 1, 1, 1));
        //     cars.Add(c);        
        // }

        sim.Init(cars);
    }

    void reader()
    {
        XmlDocument doc = new XmlDocument();

        //Console.WriteLine("CALLED");
        //XDocument xml = XDocument.Load("./geo-milev.osm");

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        
        Dictionary<string, Dictionary<string, string>> nodes = new Dictionary<string, Dictionary<string, string>>();

        //Dictionary<string, Dictionary<string, Dictionary<string, string>> wayMap = new Dictionary<string, Dictionary<string, Dictionary<string, string>>();
        List<List<Dictionary<string, Dictionary<string, string>>>> list = new List<List<Dictionary<string, Dictionary<string, string>>>>();
        XmlReader reader = XmlReader.Create("./berlin.osm", settings);
        
        reader.ReadToFollowing("node");
        string id = "";
        int lights = 0;
        do
        {   
            if(reader.IsStartElement()) {
                if(reader.Name == "node"){
                    id = reader.GetAttribute("id");
                    string lat = reader.GetAttribute("lat");
                    string lon = reader.GetAttribute("lon");
                    nodes[id] = new Dictionary<string, string>();
                    nodes[id]["lat"] = lat;
                    nodes[id]["lon"] = lon;
                    nodes[id]["lights"] = "false";

                } else if(reader.Name == "tag") {
                    if(reader.GetAttribute("k") == "highway" && reader.GetAttribute("v") == "traffic_signals") {
                        lights++;
                        nodes[id]["lights"] = "true";
                    }
                }
            }
            reader.Read();
        } while (reader.Name != "way");
        
        int curSegment = 0;
        int lastSegment = 0;

        int curPoints = 0;
        string lastRef = "";
        
        bool tagLast = false;

        List<Node> currentNodes = new List<Node>();
        //List<Edge> currentEdge = new List<Edge>();
        Debug.Log("DEEDE");
        float lat_start = 0;
        float lon_start = 0;
        bool firstRoad = true;
        do
        {   
            if(reader.IsStartElement()) {
                do{
                    reader.Read();
                    if(reader.Name == "nd") {
                        if(tagLast) {
                            currentNodes = new List<Node>();
                            tagLast = false;
                            curPoints = 0;
                        }

                        string newRef = reader.GetAttribute("ref");


                        if(curPoints == 1) {
                            if (firstRoad) {
                                lat_start = float.Parse(nodes[lastRef]["lat"]);
                                lon_start = float.Parse(nodes[lastRef]["lon"]);
                                firstRoad = false;
                            } 
                            float lat_s = float.Parse(nodes[lastRef]["lat"]);
                            float lon_s = float.Parse(nodes[lastRef]["lon"]);
                            bool lights_s = nodes[lastRef]["lights"] == "true";

                            float lat_e = float.Parse(nodes[newRef]["lat"]);
                            float lon_e = float.Parse(nodes[newRef]["lon"]);
                            bool lights_e = nodes[newRef]["lights"] == "true";

                            int scale = 100000;

                            Node start = new Node(new Vector3((lat_s - lat_start) * scale, 0, (lon_s - lon_start) * scale), lights_s);
                            
                            Node end = new Node(new Vector3((lat_e - lat_start) * scale, 0, (lon_e - lon_start) * scale), lights_e);

                            currentNodes.Add(start);
                            currentNodes.Add(end);

                            curPoints = 0;
                            curSegment++;
                        }
                        lastRef = newRef;
                        curPoints++;
                    }
                    else if(reader.Name == "tag"){
                        tagLast = true;
                        if(reader.GetAttribute("k") == "highway" && reader.GetAttribute("v") != "footway") {
                            reader.Read();
                            for(int j = 0; j < currentNodes.Count; j+=2) {
                                map.addNode(currentNodes[j]);
                                map.addNode(currentNodes[j+1]);
                                Edge edge = new Edge(currentNodes[j], currentNodes[j+1], 1, 1, 50, reader.GetAttribute("v") + currentNodes[j].position.x + " " + currentNodes[j].position.z +" "+ currentNodes[j+1].position.x + " " + currentNodes[j+1].position.z);
                                map.addEdge(edge);
                                Debug.Log(edge);
                            }  
                        }
                    }
                    //curPoints = 0;
                } while (reader.Name != "way");
            }
            lastSegment = curSegment;
            reader.Read();
        } while (reader.Name != "relation");

       // Console.WriteLine(list[0][0])
    }

    // Update is called once per frame
    void Update()
    {
        sim.Step();
    }

}
