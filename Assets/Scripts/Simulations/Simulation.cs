using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulations
{
    public abstract class Simulation
    {
        public List<Car> cars;
        public TrafficLight tf;
        public Map map;

        public Car getCarInfront(Car c, int direction) {
            return c.road.getCarInfront(c, direction);
        }
        /*
        public List<List<float>> getPoints(float x, float z) {
            int rows = 60;
            int columns = 60;
            List<List<float>> points = new List<List<int, float>>();

            for(int row = 0; row < rows; row++) {
                for(int column = 0; column < columns; column++) {
                    foreach (Car c in cars) {
                        Vector3 vec = c.WorldCoords();

                        if(vec.x > (row + x) && vec.x < (row  + x + 1) && (vec.z + z) > column && (vec.z + z + 1) < column) {
                            points[row][column][0] = 1;
                            points[row][column][1] = c.velocity;
                        } else {
                            points[row][column][0] = 0;
                            points[row][column][1] = 0;
                        }

                    }
                }
            }

            return points;
        }*/

        public List<Edge> calculatePath(Node startingNode, Node destination)
        {
            float[] dist = new float[map.nodeCount];
            int[] prev = new int[map.nodeCount];
            //Debug.Log(map.nodeCount);
            List<Node> allNodes = new List<Node>(map.nodes.Values.ToList());
            //Debug.Log(allNodes.Count);
            foreach (Node n in allNodes)
            {
                //Debug.Log(n.AddId + " " + map.nodeNeighbours[n.AddId].Count);
                if(startingNode.AddId != n.AddId)
                {
                    dist[n.AddId] = float.MaxValue;
                    prev[n.AddId] = -1;
                }
            }
            dist[startingNode.AddId] = 0;
            prev[startingNode.AddId] = -1;
            List<Node> q = new List<Node>();
            List<Node> s = new List<Node>();
            foreach(Node n in allNodes)
            {
                q.Add(n);
            }
            Debug.Log(q.Count);
            while(q.Count > 0) {
                Node u = null;
                float min = float.MaxValue;

                foreach(Node n in q)
                {
                    if(dist[n.AddId] < min)
                    {
                        min = dist[n.AddId];
                        u = n;
                    }
                    //Debug.Log("IN");
                    //Debug.Log(min);
                }
                q.Remove(u);
                s.Add(u);
                foreach (Tuple<Node, float> t in map.nodeNeighbours[u.AddId])
                {
                    if(dist[u.AddId] + t.Item2 < dist[t.Item1.AddId]){
                        dist[t.Item1.AddId] = dist[u.AddId] + t.Item2;
                        prev[t.Item1.AddId] = u.AddId;
                    }
                }
            }
            List<Edge> path = new List<Edge>();
            int AddIdx = destination.AddId;
            while(prev[AddIdx] != -1)
            {
                foreach(Edge e in map.edges)
                {
                    if(e.endNode.AddId == AddIdx && e.startNode.AddId == prev[AddIdx])
                    {
                        path.Add(e);
                    }
                }
                AddIdx = prev[AddIdx];
            }
            return path;
        }

        public abstract void Init(List<Car> cars, Map map, TrafficLight tf);
        public abstract void Step(float deltaTime);
    }
}