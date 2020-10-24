using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node startNode;
    public Node endNode; 
    public int forwardLanes;
    public int backwardLanes;
    public int maxSpeed;
    public int leght;
    public Stack<Automobile> cars; 
}
