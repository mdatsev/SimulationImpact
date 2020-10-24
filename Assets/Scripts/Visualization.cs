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

    private Simulation sim;
    private List<Car> cars = new List<Car>();

    // Start is called before the first frame update
    void Start()
    {
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
            c.changeRoad(new Edge(new Node(new Vector3(0,0,0)), new Node(new Vector3(1,0,1)), 1, 1, 1));
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
