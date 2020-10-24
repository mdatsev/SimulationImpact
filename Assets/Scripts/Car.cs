using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car : MonoBehaviour
    {
        public double velocity = 1;
        public double position = 0;
        public int lane = 0;
        public Edge road;
        public List<Edge> path = new List<Edge>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Move() {
            Edge newEdge = null;
            if(path.Count > 0)
            {
                newEdge = path[path.Count - 1];
            }
            position += velocity;
            if(newEdge == null && position > road.length)
            {
                return;
            }
            //Debug.Log(velocity);
            if(position > road.length) {
                changeRoad(newEdge);
            }
            double n = (position) / road.length;
            //Debug.Log(n);
            transform.position = road.startNode.position + (road.endNode.position - road.startNode.position) * (float)n;
            //Debug.Log(transform.position);
        }
        public void changeRoad(Edge newRoad) {
            if (road != null && newRoad != null) {
                position -= road.length;
            }
            if(newRoad != null) {
                path.RemoveAt(path.Count - 1);
                Debug.Log(path.Count);
                road = newRoad;
                transform.rotation = Quaternion.AngleAxis((float)Math.Atan2((road.direction.x), (road.direction.z)) * (180F / (float)Math.PI), Vector3.up);
                Debug.Log(newRoad.length);
            }
        }
    }
}
