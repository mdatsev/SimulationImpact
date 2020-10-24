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
    public GameObject sidewalk;
    public Map map = new Map();
    public float decorationChance = 0.5f;

    private Simulation sim;
    private List<Car> cars = new List<Car>();
    private GameObject streetWire;

    // Start is called before the first frame update
    void Start()
    {
        streetWire = new GameObject("StreetWire");
        GameObject streets = new GameObject("Streets");
        GameObject sidewalks = new GameObject("Sidewalks");
        GameObject decorations = new GameObject("Decorations");
        streets.transform.parent = streetWire.transform;
        sidewalks.transform.parent = streetWire.transform;
        decorations.transform.parent = streetWire.transform;

        // reader();
        List<Edge> edg = map.edges;
        Node n1 = new Node(new Vector3(5, 0, 0));
        Node n2 = new Node(new Vector3(10, 0, 10));

        edg.Add(new Edge(n1, n2, 1, 1, 60));
        Debug.Log(edg.Count);

        GameObject[] decorationListArray = Resources.LoadAll<GameObject>("Prefabs/Decorations");
        List<GameObject> decorationList = decorationListArray.ToList();

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
                Instantiate(sidewalk, pos + normal * 3, rotation, sidewalks.transform);
                rotation *= Quaternion.Euler(0, 180, 0);
                Instantiate(sidewalk, pos - normal * 3, rotation, sidewalks.transform);

                if (UnityEngine.Random.Range(0, 1) < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos - normal * 3, rotation, decorations.transform);
                }
                if (UnityEngine.Random.Range(0, 1) < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos + normal * 3, rotation, decorations.transform);
                }
            }
        }
    
        if (useDummySim) {
            sim = new SimulationDummy();
        } else {
            sim = new SimulationImpact();
        }

        startingPoints.Add(new Vector2(0,0));
        startingPoints.Add(new Vector2(1,1));

        GameObject[] carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        List<GameObject> carList = carListArray.ToList();
        foreach (Vector2 p in startingPoints) {
            GameObject car = Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], new Vector3(p.x, 0, p.y), Quaternion.identity);
            Car c = car.GetComponent<Car>();
            c.changeRoad(new Edge(new Node(new Vector3(0,0,0)), new Node(new Vector3(1,0,1)), 1, 1, 1));
            cars.Add(c);        
        }

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
        XmlReader reader = XmlReader.Create("./geo-milev.osm", settings);
        
        reader.ReadToFollowing("node");
        do
        {   
            if(reader.IsStartElement()) {
                if(reader.Name == "node"){
                    string id = reader.GetAttribute("id");
                    string lat = reader.GetAttribute("lat");
                    string lon = reader.GetAttribute("lon");
                    nodes[id] = new Dictionary<string, string>();
                    nodes[id]["lat"] = lat;
                    nodes[id]["lon"] = lon;
                } else if(reader.Name == "tag") {
                    
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

        do
        {   
            if(reader.IsStartElement()) {
                do{
                    reader.Read();
                    if(reader.Name == "nd") {
                        if(tagLast) {
                            currentNodes = new List<Node>();
                        } else {
                            tagLast = false;
                        }

                        string newRef = reader.GetAttribute("ref");
                        if(curPoints == 1) {

                            float lat_s = float.Parse(nodes[lastRef]["lat"]);
                            float lon_s = float.Parse(nodes[lastRef]["lon"]);

                            float lat_e = float.Parse(nodes[newRef]["lat"]);
                            float lon_e = float.Parse(nodes[newRef]["lon"]);
                            int scale = 10000;

                            Node start = new Node(new Vector3((float)(lat_s - Math.Floor(lat_s)) * scale, 0, (float)(lon_s - Math.Floor(lon_s)) * scale));
                            Node end = new Node(new Vector3((float)(lat_e - Math.Floor(lat_e)) * scale, 0, (float)(lon_e - Math.Floor(lon_e)) * scale));

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
                        for(int i = lastSegment; i < curSegment; i++) {
                            if(reader.GetAttribute("k") == "highway") {
                                for(int j = 0; j < currentNodes.Count; j+=2) {
                                    map.addNode(currentNodes[j]);
                                    map.addNode(currentNodes[j+1]);
                                    
                                    Edge edge = new Edge(currentNodes[j], currentNodes[j+1], 1, 1, 50);

                                    map.addEdge(edge);

                                }  

                            }
                            //list[i][2]["tags"][reader.GetAttribute("k")] = reader.GetAttribute("v");
                        }
                    }
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
