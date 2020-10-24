using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public abstract class Simulation
    {
        public List<Car> cars;
        public Map map;

        public Car getCarInfront(Car c) {
            return null;
        }

        private List<Edge> calculatePath(Node startingNode, Node destination)
        {
            float[] dist = new float[map.nodeCount];
            int[] prev = new int[map.nodeCount];
            List<Node> allNodes = new List<Node>(map.Nodes.Keys);
            foreach (Node n in allNodes)
            {
                if(startingNode.Id != n.id)
                {
                    dist[n.Id] = float.MaxValue;
                    prev[n.id] = -1;
                }
            }
            dist[startingNode.Id] = 0;
            prev[startingNode.Id] = -1;
            List<Node> q = new List<Node>();
            List<Node> s = new List<Node>();
            foreach(Node n in allNodes)
            {
                q.Add(n);
            }
            while(q.Count > 0) {
                Node u;
                float min = float.MaxValue;

                foreach(Node n in q)
                {
                    if(dist[n.id] < min)
                    {
                        min = dist[n.id];
                        u = n;
                    }
                }
                q.Remove(u);
                s.Add(u);

                foreach (Tuple<Node, float> t in map.nodeNeighbours[u.Id])
                {
                    if(dist[u.Id] + t.Item2 < dist[t.Item1.Id]){
                        dist[t.Item1.Id] = dist[u.Id] + t.Item2;
                        prev[t.Item1.Id] = u.Id;
                    }
                }
            }
            List<Edge> path = new List<Edge>();
            int idx = destination.Id;
            while(prev[idx] != -1)
            {
                foreach(Edge e in map.edges)
                {
                    if(e.endNode.Id == idx && e.startNode.Id == prev[idx])
                    {
                        path.Add(e);
                    }
                }
                idx = prev[idx];
            }
            return path;
        }

        public abstract void Init(List<Car> cars);
        public abstract void Step();
    }
}