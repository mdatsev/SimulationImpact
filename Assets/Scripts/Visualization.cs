﻿using System.Collections;
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
        List<Edge> edg = new List<Edge>();
        Node n1 = new Node(new Vector3(0, 0, 0));
        Node n2 = new Node(new Vector3(10, 0, 10));

        edg.Add(new Edge(n1, n2, 1, 1, 60));
        //Node point1,point2;

        foreach (Edge e in edg)
        {
            int numofprefs = (int)e.length / 4;

            //Instantiate(street, new Vector3(e.getStart().position.x, 0, e.getStart().position.z), Quaternion.LookRotation(e.direction));
            for (int i=1; i <= numofprefs; i++)
            {
               Instantiate(street, new Vector3(e.direction.x + 2*i, 0, e.direction.z + 2*i), Quaternion.identity);
            }
            //Instantiate(street, new Vector3(e.getEnd().position.x, 0, e.getEnd().position.z), Quaternion.LookRotation(e.direction));
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
            GameObject car = Instantiate(carList[Random.Range(0, carList.Count)], new Vector3(p.x, 0, p.y), Quaternion.identity);
            Car c = car.GetComponent<Car>();
            c.changeRoad(new Edge(new Node(new Vector3(0,0,0)), new Node(new Vector3(1,0,1)), 1, 1, 1));
            cars.Add(c);        
        }

        sim.Init(cars);
    }

    // Update is called once per frame
    void Update()
    {
        sim.Step();
    }

}
