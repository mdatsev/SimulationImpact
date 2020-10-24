using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationRun : MonoBehaviour
{
    public GameObject simulation;
    public List<Vector2> startingPoints = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        startingPoints.Add(new Vector2(0,0));
        startingPoints.Add(new Vector2(1,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
