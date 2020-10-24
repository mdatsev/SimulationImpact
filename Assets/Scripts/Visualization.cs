using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Simulations;

public class Visualization : MonoBehaviour
{
    public GameObject SimulationManager;
    public bool useDummySim;
    public List<Vector2> startingPoints = new List<Vector2>();
    public GameObject street;
    private Simulation sim;
    private List<Car> cars = new List<Car>();

    // Start is called before the first frame update
    void Start()
    {
        SimulationRun simulation = SimulationManager.GetComponent<SimulationRun>();
        List<Vector2> startingPoints = simulation.startingPoints;
        List<Edge> edg = simulation.edges;
        //Node point1,point2;

        foreach (Edge e in edg)
        {
            int numofprefs = (int)e.lenght / 4;
            for (int i=0; i < numofprefs; i++)
            {
                Debug.Log("MAMA Ti Deba + " + e.getStart().getPos().x + " ;; + "+ e.getStart().getPos().z);
                Instantiate(street, new Vector3(e.getStart().getPos().x + i, 0, e.getStart().getPos().z + i), Quaternion.identity);
            }
        }

        if (useDummySim) {
            sim = new SimulationImpact();
        } else {
            sim = new SimulationDummy();
        }
        

        startingPoints.Add(new Vector2(0,0));
        startingPoints.Add(new Vector2(1,1));

        GameObject[] carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        List<GameObject> carList = carListArray.ToList();

        foreach (Vector2 p in startingPoints) {
            GameObject car = Instantiate(carList[Random.Range(0, carList.Count)], new Vector3(p.x, 0, p.y), Quaternion.identity);
            Car c = car.GetComponent<Car>();
            Debug.Log(c.speed);

            cars.Add(car.GetComponent<Car>());        
        }

        sim.Init(cars);
    }

    // Update is called once per frame
    void Update()
    {
        sim.Step();
    }

}
