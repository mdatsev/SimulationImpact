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
    public List<double> startingPoints = new List<double>();
    public GameObject street;
    public List<StreetTile> streetTiles;
    public GameObject building;
    public GameObject sidewalk;
    public GameObject traficL;
    public Map map = new Map();
    public float decorationChance = 0.2f;
    public float buildingChance = 0.2f;
    int frames = 0;
    private Simulation sim;
    private List<Car> cars = new List<Car>();
    private List<GameObject> gameCars = new List<GameObject>();
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
        /*
        Node test1 = new Node(new Vector3(0, 0, 0), false);
        Node test2 = new Node(new Vector3(500, 0, 1), false);
        Node test3 = new Node(new Vector3(1000, 0, 100), false);
        Edge edgee = new Edge(test1, test2, 1, 1, 1, "");
        Edge edgee2 = new Edge(test2, test3, 1, 1, 1, "");    
        map.addNode(test1);
        map.addNode(test2);
        map.addNode(test3);
        //Edge edge = new Edge(currentNodes[j], currentNodes[j + 1], 1, 1, 50, reader.GetAttribute("v") + currentNodes[j].position.x + " " + currentNodes[j].position.z + " " + currentNodes[j + 1].position.x + " " + currentNodes[j + 1].position.z);
        map.addEdge(edgee);
        map.addEdge(edgee2);
        map.addEdge(edgee2);
        */

        List<Edge> edg = map.edges;

        //Node n1 = new Node(new Vector3(5, 0, 0), true);
        //Node n2 = new Node(new Vector3(10, 0, 10), true);

        //edg.Add(new Edge(n1, n2, 1, 1, 60, ""));
        //Debug.Log(edg.Count);

        GameObject[] decorationListArray = Resources.LoadAll<GameObject>("Prefabs/Decorations");
        List<GameObject> decorationList = decorationListArray.ToList();
        System.Random rand = new System.Random();

        GameObject[] buildingListArray = Resources.LoadAll<GameObject>("Prefabs/Buildings");
        List<GameObject> buildingList = buildingListArray.ToList();

        foreach (Edge e in edg)
        {
            StreetTile street = streetTiles[Math.Min(e.forwardLanes + e.backwardLanes - 1, 1)];
            int prefsNum = (int)Math.Ceiling(e.length / street.length);
            bool complicatedEnd = false;
            bool complicatedStart = false;
            if (map.nodeNeighbours[e.endNode.AddId].Count > 2) { complicatedEnd = true; }
            if (map.nodeNeighbours[e.startNode.AddId].Count > 2) { complicatedStart = true;  }

            for (int i=0; i <= prefsNum; i++)
            {
                float x = e.startNode.position.x + (e.direction.x)*((float)i / prefsNum);
                float z = e.startNode.position.z + (e.direction.z)*((float)i / prefsNum);
                Quaternion rotation = Quaternion.AngleAxis( -90 + (float)Math.Atan2((e.direction.x), (e.direction.z))*(180F/(float)Math.PI), Vector3.up);
                Vector3 pos = new Vector3(x, 0, z);
                Vector3 normal = Vector3.Cross(e.direction, new Vector3(0,1,0)).normalized;
                Vector3 decorOffset = normal * (street.width / 2 + 1);
                Vector3 buildingOffset = normal * (street.width / 2 + 2) * 2;

                Instantiate(street, pos, rotation, streets.transform);
                
                if(e.startNode.traficLight && (i == 0 || i == prefsNum)) {
                    Instantiate(traficL, pos + decorOffset, rotation, traficLights.transform);
                }

                if (rand.NextDouble() < buildingChance && i > 3)
                {
                    Instantiate(buildingList[UnityEngine.Random.Range(0, buildingList.Count)]
                        , pos + buildingOffset, rotation, buildings.transform);
                }

                Instantiate(sidewalk, pos + decorOffset, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos + decorOffset, rotation, decorations.transform);
                }

                rotation *= Quaternion.Euler(0, 180, 0);
    //right side
                Instantiate(sidewalk, pos - decorOffset, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos - decorOffset, rotation, decorations.transform);
                }
                
                if (rand.NextDouble() < buildingChance && i < prefsNum - 3)
                {
                    Instantiate(buildingList[UnityEngine.Random.Range(0, buildingList.Count)]
                        , pos - buildingOffset, rotation, buildings.transform);
                }
            }
        }
    
        if (useDummySim) {
            sim = new SimulationDummy();
        } else {
            sim = new SimulationImpact();
        }

        /*startingPoints.Add(new Vector2(0,0));
        /*
        double startingLength = 0;
        double incrase = 15;
        for(int i = 0; i < 5; i++)
        {
            startingPoints.Add(startingLength);
            startingLength += incrase;
        }
        startingPoints.Add(startingLength + 50);
        //startingPoints.Add(new Vector2(1,1));

        TrafficLight tf = new TrafficLight(edgee);

        GameObject[] carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        List<GameObject> carList = carListArray.ToList();
        foreach (double p in startingPoints) {
            Car c = new Car();
            c.position = p;
            c.velocity = (double)UnityEngine.Random.Range(2.0F, 3.0F);
            c.path.Add(edgee2);
            c.path.Add(edgee);
            c.changeRoad(edgee);
            gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c.WorldCoords(), Quaternion.identity));
            cars.Add(c);    
        }
        */
        sim.Init(cars, null);
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

        List<string> currentNodes = new List<string>();
        //List<Edge> currentEdge = new List<Edge>();

        float lat_start = 0;
        float lon_start = 0;
        bool firstRoad = true;
        
        int forwardLanes = 1;
        int backwardLanes = 1;
        
        bool toAdd = false;
        do
        {   
            if(reader.IsStartElement()) {
                do{
                    reader.Read();
                    if(reader.Name == "nd") {
                        if(tagLast) {
                            currentNodes = new List<string>();
                            tagLast = false;
                            curPoints = 0;
                        }
                        string newRef = reader.GetAttribute("ref");


                        if(curPoints == 1) {
                            float scaleLat = 111320;
                            float lat_s = float.Parse(nodes[lastRef]["lat"]) * scaleLat;
                            float scaleLon = 40075000 * (float)Math.Cos(lat_s * (float)Math.PI / 180f) / 360f;
                            float lon_s = float.Parse(nodes[lastRef]["lon"]) * scaleLon;

                            if (firstRoad) {
                                lat_start = lat_s;
                                lon_start = lon_s;
                                firstRoad = false;
                            } 
                            bool lights_s = nodes[lastRef]["lights"] == "true";

                            float lat_e = float.Parse(nodes[newRef]["lat"]) * scaleLat;
                            float lon_e = float.Parse(nodes[newRef]["lon"]) * scaleLon;
                            bool lights_e = nodes[newRef]["lights"] == "true";

                            Node start = new Node(new Vector3((lat_s - lat_start), 0, (lon_s - lon_start)), lights_s);
                            start.Id = lastRef;
                            Node end = new Node(new Vector3((lat_e - lat_start), 0, (lon_e - lon_start)), lights_e);
                            end.Id = newRef;
                            currentNodes.Add(lastRef);
                            currentNodes.Add(newRef);
                            map.addNode(start);
                            map.addNode(end);
                            curPoints = 0;
                            curSegment++;
                        }
                        lastRef = newRef;
                        curPoints++;
                    }
                    else if(reader.Name == "tag"){
                        tagLast = true;

                        if(reader.GetAttribute("k") == "highway" && reader.GetAttribute("v") != "footway") {
                            //reader.Read();
                            //Debug.Log(currentNodes.Count);
                            toAdd = true;
                        }

                        if(reader.GetAttribute("k") == "lanes" && toAdd) {
                            forwardLanes = int.Parse(reader.GetAttribute("v"));
                        }
                        if(reader.GetAttribute("k") == "oneway" && toAdd) {
                            if(reader.GetAttribute("v") == "no") {
                                backwardLanes = forwardLanes;
                            } else {
                                backwardLanes = 0;
                            }
                        }
                    }
                    //curPoints = 0;
                } while (reader.Name != "way");
                if(toAdd){
                    for(int j = 0; j < currentNodes.Count; j+=2) {
                        Node startNode = map.nodes[currentNodes[j]];
                        Node endNode = map.nodes[currentNodes[j+1]];

                        Edge edge = new Edge(startNode, endNode, forwardLanes, backwardLanes, 50, reader.GetAttribute("v") + startNode.position.x + " " + startNode.position.z +" "+ endNode.position.x + " " + endNode.position.z);
                        map.addEdge(edge);
                    }
                }
                toAdd = false;
                backwardLanes = 1;
                forwardLanes = 1;
            }
            lastSegment = curSegment;
            reader.Read();
        } while (reader.Name != "relation");

       // Console.WriteLine(list[0][0])
    }

    // Update is called once per frame
    void Update()
    {
        frames++;



        for(int i = 0; i < cars.Count; i++)
        {
            gameCars[i].transform.position = cars[i].WorldCoords();
            gameCars[i].transform.rotation = cars[i].worldRotation();
        }
        sim.Step(frames);
        /*cars[cars.Count - 1].velocity = 0;
        if (cars[cars.Count - 1].velocity < 0)
        {
            cars[cars.Count - 1].velocity = 0;
        }*/
    }

}
