using System;
using System.Collections;
using System.Collections.Generic;

public class Map
{
    public Dictionary<string, Node> nodes;
    public List<Edge> edges;
    public List<List<Tuple<Node, float>>> nodeNeighbours;
    public int nodeCount = 0;
    public Map()
    {
        nodes = new Dictionary<string, Node>();
        edges = new List<Edge>();
        nodeNeighbours = new List<List<Tuple<Node, float>>>();
 
    }
    public void addNode(Node node) {
        if (!nodes.ContainsKey(node.Id))
        {
            node.AddId = nodeCount;
            nodes.Add(node.Id, node);
            nodeCount++;
            nodeNeighbours.Add(new List<Tuple<Node, float>>());
        }
        else
        {
            //Debug.Log(nodeNeighbours[node.Id].Count);
        }
    }

    private void addNeighbourIfExists(Node s, Node e, float length) {
        if (nodeNeighbours.Count > s.AddId && nodeNeighbours.Count > e.AddId) {
            nodeNeighbours[s.AddId].Add(new Tuple<Node, float>(e, length));
            nodeNeighbours[e.AddId].Add(new Tuple<Node, float>(s, length));
        }
    }

    public void addEdge(Edge edge) {
        edges.Add(edge);
        addNeighbourIfExists(edge.startNode, edge.endNode, edge.length);
        //Debug.Log(nodeNeighbours[edge.startNode.AddId].Count);
    }


}
