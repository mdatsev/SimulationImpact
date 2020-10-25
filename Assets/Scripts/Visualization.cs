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

        // reader();
        Map map = new Map();

        GameObject[] carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        List<GameObject> carList = carListArray.ToList();


        Node test1 = new Node(new Vector3(0, 0, 0), false);
        Node test2 = new Node(new Vector3(100, 0, 0), false);
        Node test3 = new Node(new Vector3(0, 0, 100), false);
        Node test4 = new Node(new Vector3(-100, 0, 0), false);
        Node test5 = new Node(new Vector3(0, 0, -100), false);

        Edge edgeE = new Edge(test2, test1, 1, 1, 1, "");
        Edge edgeN = new Edge(test3, test1, 1, 1, 1, "");
        Edge edgeW = new Edge(test4, test1, 1, 1, 1, "");
        Edge edgeS = new Edge(test5, test1, 1, 1, 1, "");

        map.addNode(test1);
        map.addNode(test2);
        map.addNode(test3);
        map.addNode(test4);
        map.addNode(test5);
        
        map.addEdge(edgeE);
        map.addEdge(edgeN);
        map.addEdge(edgeW);
        map.addEdge(edgeS);
        
            List<Edge> path = new List<Edge>();
        Car c1 = new Car();
        path.Add(edgeW);
        //path.Add(edgeS);
        c1.path = path;
        c1.position = 0;
        c1.changeRoad(edgeW);
        gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c1.WorldCoords(), Quaternion.identity));

        Car c2 = new Car();
        path = new List<Edge>();
        path.Add(edgeS);
        //path.Add(edgeW);
        c2.path = path;
        c2.position = 0;
        c2.changeRoad(edgeS);
        gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c2.WorldCoords(), Quaternion.identity));

        Car c3 = new Car();
        path = new List<Edge>();
        path.Add(edgeE);
        //path.Add(edgeW);
        c3.path = path;
        c3.position = 0;
        c3.changeRoad(edgeE);
        gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c3.WorldCoords(), Quaternion.identity));

        Car c4 = new Car();
        path = new List<Edge>();
        path.Add(edgeW);
        //path.Add(edgeE);
        c4.path = path;
        c4.position = 0;
        c4.changeRoad(edgeW);
        gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c4.WorldCoords(), Quaternion.identity));
    
        //cars.Add(c1);
        cars.Add(c2);
        cars.Add(c3);
        cars.Add(c4);
        

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

        RaycastHit hit;

        foreach (Edge e in edg)
        {
            Debug.Log("asd");
            StreetTile street = streetTiles[Math.Min(e.forwardLanes + e.backwardLanes - 1, 1)];
            int prefsNum = (int)Math.Ceiling(e.length / street.length);

            for (int i = 0; i <= prefsNum; i++)
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

                Instantiate(sidewalk, pos + decorOffset, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos + decorOffset, rotation, decorations.transform);
                }

                bool hitted = Physics.Linecast(pos + buildingOffset, pos + buildingOffset - Vector3.up, out hit);
                if (hitted) Debug.Log( hit.collider.gameObject.name);

                if (rand.NextDouble() < buildingChance && !hitted)
                {
                    Instantiate(buildingList[UnityEngine.Random.Range(0, buildingList.Count)]
                        , pos + buildingOffset, rotation, buildings.transform);
                }

                rotation *= Quaternion.Euler(0, 180, 0);
    //right side
                Instantiate(sidewalk, pos - decorOffset, rotation, sidewalks.transform);
                if (rand.NextDouble() < decorationChance) {
                    Instantiate(decorationList[UnityEngine.Random.Range(0, decorationList.Count)]
                        , pos - decorOffset, rotation, decorations.transform);
                }
                
                if (rand.NextDouble() < buildingChance && !hitted)
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

        sim.Init(cars, map, new TrafficLight(edgeS));



/*

        double startingLength = 0;
        double incrase = 10;
        for (int i = 0; i < 2; i++)
        {
            startingPoints.Add(0);
        }
        //startingPoints.Add(new Vector2(1,1));

        TrafficLight tf = new TrafficLight(edg[45]);

        List<GameObject> carList = carListArray.ToList();
        sim.Init(cars, map, tf);
        List<Edge> path = sim.calculatePath(edg[32].startNode, edg[4].endNode);
        foreach (double p in startingPoints)
        {
        List<Edge> path = sim.calculatePath(edg[32].startNode, edg[47].endNode);
        List<Edge> path2 = sim.calculatePath(edg[47].startNode, edg[32].endNode);
       
            Car c = new Car();
            c.position = 0;
            //c.velocity = (double)UnityEngine.Random.Range(2.0F, 3.0F);
            c.path = path;
            c.changeRoad(c.path[c.path.Count - 1]);
            gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c.WorldCoords(), Quaternion.identity));
            // cars.Add(c);
             Car c1 = new Car();
            c1.position = 0;
            //c.velocity = (double)UnityEngine.Random.Range(2.0F, 3.0F);
            c1.path = path2;
            c1.changeRoad(c1.path[c1.path.Count - 1]);
            gameCars.Add(Instantiate(carList[UnityEngine.Random.Range(0, carList.Count)], c.WorldCoords(), Quaternion.identity));
            cars.Add(c1);
        


*/
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
        XmlReader reader = XmlReader.Create("./geo-milev-2.osm", settings);
        
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
        Dictionary<string, Node> uniqueNodes = new Dictionary<string, Node>();
        //List<Edge> currentEdge = new List<Edge>();

        float x_start = 0;
        float y_start = 0;
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
                            //float scaleLat = 111320
                            const float R = 6371000;
                            float lat_s = float.Parse(nodes[lastRef]["lat"]);
                            float lon_s = float.Parse(nodes[lastRef]["lon"]);
                            float rad = (float)Math.PI / 180f;
                             
                            float xs = R * (float)Math.Cos(lat_s * rad) * (float)Math.Cos(lon_s * rad);
                            float ys = R * (float)Math.Cos(lat_s * rad) * (float)Math.Sin(lon_s * rad);


                            // float scaleLon = 4007500 * (float)Math.Cos(lat_s * (float)Math.PI / 180f) / 360f;

                            if (firstRoad) {
                                x_start = xs;
                                y_start = ys;
                                firstRoad = false;
                            } 
                            bool lights_s = nodes[lastRef]["lights"] == "true";

                            float lat_e = float.Parse(nodes[newRef]["lat"]);
                            float lon_e = float.Parse(nodes[newRef]["lon"]);

                            float xe = R * (float)Math.Cos(lat_e * rad) * (float)Math.Cos(lon_e * rad);
                            float ye = R * (float)Math.Cos(lat_e * rad) * (float)Math.Sin(lon_e * rad);
                            
                            bool lights_e = nodes[newRef]["lights"] == "true";

                            Node start = new Node(new Vector3((xs - x_start), 0, (ys - y_start)), lights_s);
                            start.Id = lastRef;
                            Node end = new Node(new Vector3((xe - x_start), 0, (ye - y_start)), lights_e);
                            end.Id = newRef;
                            currentNodes.Add(lastRef);
                            currentNodes.Add(newRef);
                            if(!uniqueNodes.ContainsKey(start.Id))
                            {
                                uniqueNodes.Add(start.Id, start);
                            }
                            if(!uniqueNodes.ContainsKey(end.Id))
                            {
                                uniqueNodes.Add(end.Id, end);
                            }
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
                        Node startNode = uniqueNodes[currentNodes[j]];
                        Node endNode = uniqueNodes[currentNodes[j+1]];
                        map.addNode(startNode);
                        map.addNode(endNode);
                        Edge edge = new Edge(startNode, endNode, forwardLanes, backwardLanes, 50, reader.GetAttribute("v") + startNode.position.x + " " + startNode.position.z +" "+ endNode.position.x + " " + endNode.position.z);
                        //Debug.Log(edge.length);

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
        sim.Step(Time.deltaTime);
        /*cars[cars.Count - 1].velocity = 0;
        if (cars[cars.Count - 1].velocity < 0)
        {
            cars[cars.Count - 1].velocity = 0;
        }*/
    }

}
