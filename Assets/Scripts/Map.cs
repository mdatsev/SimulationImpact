using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Dictionary<int, Node> nodes;
    public List<Edge> edges;
    public List<List<Tuple<Node, float>>> nodeNeighbours;
    public int nodeCount = 0;
    public Map()
    {
        nodes = new Dictionary<int, Node>();
        edges = new List<Edge>();
 
    }
    public void addNode(Node node) {
        if (!nodes.ContainsKey(node.Id))
        {
            nodes.Add(nodeCount, node);
            node.Id = nodeCount;
            nodeCount++;
            nodeNeighbours.Add(new List<Tuple<Node, float>>());
        }
    }

    private void addNeighbourIfExists(Node s, Node e, float length) {
        if (nodeNeighbours.Count > s.Id && nodeNeighbours.Count > e.Id) {
            nodeNeighbours[s.Id].Add(new Tuple<Node, float>(e, length));
            nodeNeighbours[e.Id].Add(new Tuple<Node, float>(s, length));
        }
    }

    public void addEdge(Edge edge) {
        edges.Add(edge);
        addNeighbourIfExists(edge.startNode, edge.endNode, edge.length);
    }


}
