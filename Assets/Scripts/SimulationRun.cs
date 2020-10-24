// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Simulations;
/*
public class SimulationRun : MonoBehaviour
{
    public bool useDummySim;
    public List<Vector2> startingPoints = new List<Vector2>();
    List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();
    private Simulation sim;

    // Start is called before the first frame update
    void Start()
    {
        if (useDummySim) {
            sim = new SimulationImpact();
        } else {
            sim = new SimulationDummy();
        }
        Node n1 = new Node(new Vector3(0, 0, 0));
        Node n2 = new Node(new Vector3(0, 0, 0));
        nodes.Add(n1);
        nodes.Add(n2);
        edges.Add(new Edge(n1,n2,1,1,60));
        startingPoints.Add(new Vector2(0,0));
        startingPoints.Add(new Vector2(1,1));
    }*/
// public class SimulationRun : MonoBehaviour
// {
//     public bool useDummySim;
//     public List<Vector2> startingPoints = new List<Vector2>();
//     private Simulation sim;

//     // Start is called before the first frame update
//     void Start()
//     {
//         sim.Init();

//         if (useDummySim) {
//             sim = new SimulationImpact();
//         } else {
//             sim = new SimulationDummy();
//         }

//         startingPoints.Add(new Vector2(0,0));
//         startingPoints.Add(new Vector2(1,1));
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         sim.Step();
//     }
// }
