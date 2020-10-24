using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Visualization : MonoBehaviour
{
    public GameObject SimulationManager;

    private List<GameObject> carList;  
    private GameObject[] carListArray;

    // Start is called before the first frame update
    void Start()
    {
        SimulationRun simulation = SimulationManager.GetComponent<SimulationRun>();
        List<Vector2> startingPoints = simulation.startingPoints;

        carListArray = Resources.LoadAll<GameObject>("Prefabs/Cars");
        carList = carListArray.ToList();

        foreach (Vector2 p in startingPoints) {
            Instantiate(carList[Random.Range(0, carList.Count)], new Vector3(p.x, 0, p.y), Quaternion.identity);
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
