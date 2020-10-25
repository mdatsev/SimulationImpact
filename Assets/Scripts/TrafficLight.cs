using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TrafficLight
{
    public Node node;
    public float accTime; //accumulated time
    public float changeTime;
    bool intersection;
    bool blockedEven = false;
    Tuple<float, Node>[] degree = new Tuple<float, Node>[4];

    public TrafficLight(Node n, bool intersection, Map map) {
        this.intersection = intersection;
        changeTime = 25; //+ (new Random().Next(10, 40));
        node = n;
        this.intersection = intersection;
        if (intersection && map.nodeNeighbours[node.AddId].Count == 4)
        {

            int idx = 0;
            foreach (Tuple<Node, float> t in map.nodeNeighbours[node.AddId])
            {
                degree[idx] = new Tuple<float, Node> ((float)Math.Atan2(
                    t.Item1.position.x * node.position.z - t.Item1.position.z * node.position.x,
                    t.Item1.position.x * node.position.x - t.Item1.position.z * node.position.z), t.Item1);
                idx++;
            }
            Array.Sort(degree, delegate (Tuple<float, Node> f1, Tuple<float, Node> f2)
            {
                return (int)(f1.Item1 - f2.Item1);
            });
        }
    }

    public bool canPass(Node from)
    {
        //Debug.Log(intersection);
        if(intersection)
        {
            for(int i = 0; i < 4; i++)
            {
                Debug.Log(degree[i].Item1);
                if(degree[i].Item2 == from)
                {
                    return !(blockedEven && (i == 0 || i == 1) || (!blockedEven && (i == 2 || i == 3)));
                }
            }
            return true;
        }
        else
        {
            return blockedEven;
        }
    }

    public void update(float deltaTime) {
        accTime += deltaTime;
        if (accTime > changeTime) {
            accTime -= changeTime;
            blockedEven = !blockedEven;
            Debug.Log(blockedEven);
        }
    }

}
