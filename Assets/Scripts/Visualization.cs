using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualization : MonoBehaviour
{
    public GameObject SimulationManager;
    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        SimulationRun simulation = SimulationManager.GetComponent<SimulationRun>();
        List<Vector2> startingPoints = simulation.startingPoints;

        foreach (Vector2 p in startingPoints) {
            Instantiate(car, new Vector3(p.x, 0, p.y), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCar()
    {

    }
}
