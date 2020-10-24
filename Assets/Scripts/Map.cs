using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public List<Node> nodes;
    public List<Edge> edges;

    public Map()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();

    }
    public void addNode(Node node) {
        nodes.Add(node);
    }
    public void addEdge(Edge edge) {
        edges.Add(edge);
    }


}
