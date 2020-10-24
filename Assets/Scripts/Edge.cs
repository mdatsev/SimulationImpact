using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulations;

public class Edge
{
    public Node startNode;
    public Node endNode; 
    public int forwardLanes;
    public int backwardLanes;
    public int maxSpeed;
    public float length;
    public string name;
    public Vector3 direction;
    public Stack<Car> cars;

    public Edge(Node n1, Node n2, int fl, int bl, int maxs, string n)
    {
        startNode = n1;
        endNode = n2;
        forwardLanes = fl;
        backwardLanes = bl;
        maxSpeed = maxs;
        name = n;
        float dx = n2.position.x - n1.position.x;
        float dz = n2.position.z - n1.position.z;
        direction = n2.position - n1.position;
        length = (float)Math.Sqrt((dx * dx) + (dz * dz));
    }

}
