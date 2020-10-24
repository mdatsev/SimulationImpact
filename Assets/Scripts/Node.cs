using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public string Id;
    public int AddId;
    public bool traficLight;
    public bool intersection;

    public Node(Vector3 pos, bool lights)
    {
        position = pos;
        traficLight = lights;
        intersection = false;
        Id = "-1";
    }

    public Vector3 getPos()
    {
        return position;
    }
}
