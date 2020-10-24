using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Visualization : MonoBehaviour
{
    public GameObject SimulationManager;
    public GameObject street;
    private List<GameObject> carList;  
    private GameObject[] carListArray;

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
