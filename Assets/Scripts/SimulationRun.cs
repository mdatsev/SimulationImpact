using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulations;

public class SimulationRun : MonoBehaviour
{
    public bool useDummySim;
    public List<Vector2> startingPoints = new List<Vector2>();
    private Simulation sim;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
