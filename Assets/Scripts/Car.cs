using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car
    {
        public double velocity = 1;
        public double position = 0;
        public int lane = 0;
        public Edge road;
        public List<Edge> path = new List<Edge>();

        public Vector3 WorldCoords()
        {
            double n = (position) / road.length;
            return road.startNode.position + (road.endNode.position - road.startNode.position) * (float)n;
        }

        public Quaternion worldRotation()
        {
            return Quaternion.AngleAxis((float)Math.Atan2((road.direction.x), (road.direction.z)) * (180F / (float)Math.PI), Vector3.up);
        }

        public Edge getNextRoad()
        {
            Edge res = null;
            if (path.Count > 0)
            {
                res = path[path.Count - 1];
            }
            return res;
        }

        public void Move() {
            Edge newEdge = getNextRoad();
            if(newEdge == null && position + velocity > road.length)
            {
                return;
            }
            position += (velocity / 10);
            //Debug.Log(position);
            if(position > road.length) {
                changeRoad(newEdge);
            }
            //Debug.Log(n);
            //transform.position = WorldCoords();
            //Debug.Log(transform.position);
        }
        public void changeRoad(Edge newRoad) {
            if (road != null && newRoad != null) {
                position -= road.length;
                road.RemoveCar(this);
                // Debug.Log("RemovCar");
                // Debug.Log(path.Count);
            }
            if(newRoad != null) {
                path.RemoveAt(path.Count - 1);
                //Debug.Log(path.Count);
                road = newRoad;
                road.AddCar(this);
                //Debug.Log(newRoad.length);
            }
        }
    }
}
