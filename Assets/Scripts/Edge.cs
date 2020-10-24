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
    public float lenght;
    public Vector3 direction;
    public Stack<Car> cars;

    public Edge(Node n1, Node n2,int fl,int bl,int maxs)
    {
        startNode = n1;
        endNode = n2;
        forwardLanes = fl;
        backwardLanes = bl;
        maxSpeed = maxs;
        float dx = n2.getPos().x - n1.getPos().x;
        float dz = n2.getPos().z - n1.getPos().z;
        direction.x = dx;
        direction.y = 0;
        direction.z = dz;
        lenght = (float)Math.Sqrt(dx*dx + dz*dz); 
    }

    public Node getStart()
    {
        return startNode;
    }

    public Node getEnd()
    {
        return startNode;
    }

}
