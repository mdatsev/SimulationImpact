using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Simulations;

public class AirVisualization : MonoBehaviour
{
    public GameObject Street;
    public int w = 50;
    public int h = 50;
    AirSimulation sim;
    List<GameObject> pixels = new List<GameObject>();
    // Start is called before the first frame update
    GameObject grid;

    void Start()
    {
        grid = new GameObject("PixelGrid");

        sim = new AirSimulation(w,h);
        for (int i = 0; i < sim.grid.Count; i++) {
            pixels.Add(Instantiate(Street, new Vector3(i % w, 0, i / h), Quaternion.identity, grid.transform));
        }
        sim.grid[55] = 100;
    }

    // Update is called once per frame
    void Update()
    {
        string str = "";
        for (int i = 0; i < sim.grid.Count; i++) {
            var col = pixels[i].GetComponent<Renderer>().material.color;
            float a = (float)sim.grid[i];
            pixels[i].GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(a, a, a, 1));
            // str += sim.grid[i].ToString("0.0") + " ";
        }
        // Debug.Log(str);

        sim.Step();
    }
}
