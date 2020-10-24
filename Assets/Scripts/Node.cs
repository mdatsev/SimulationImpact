using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public bool traficLight;
    public bool intersection;

    public Node(Vector3 pos)
    {
        position = pos;
        traficLight = false;
        intersection = false;
    }

    public Vector3 getPos()
    {
        return position;
    }
}
