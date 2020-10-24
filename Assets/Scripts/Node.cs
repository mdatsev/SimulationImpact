using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public int Id;
    public bool traficLight;
    public bool intersection;

    public Node(Vector3 pos)
    {
        position = pos;
        traficLight = false;
        intersection = false;
        Id = 0;
    }

    public Vector3 getPos()
    {
        return position;
    }
}
