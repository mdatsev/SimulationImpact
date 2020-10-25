using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car
    {
        public double velocity = 1;
        public double position = 0;
        public int direction = 1;
        public int lane = 0;
        public Edge road;
        public List<Edge> path = new List<Edge>();

        public Vector3 WorldCoords()
        {
            double n = (position) / road.length;
            if (direction == -1)
            {
                n = 1 - n;
            }
            var perpendicular = new Vector3(-road.direction.z, 0, road.direction.x);
            var offset = - direction * perpendicular.normalized * 1.1f;

            Debug.Log(String.Format("{0} {1} {2}", road.direction, perpendicular, offset));
            return road.startNode.position + (road.endNode.position - road.startNode.position) * (float)n + offset;
        }

        public Quaternion worldRotation()
        {
            float angleOffset = direction == 1 ? 0 : 180;
            return Quaternion.AngleAxis((float)Math.Atan2((road.direction.x), (road.direction.z)) * (180F / (float)Math.PI) + angleOffset, Vector3.up); // za pesho
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
                var realEnd = direction == 1 ? road.endNode : road.startNode;
                position -= road.length;
                road.RemoveCar(this, direction);
                Debug.Log("RemovCar");
                Debug.Log(path.Count);
                if (realEnd == newRoad.startNode) {
                    direction = 1;
                } else { // realEnd == newRoad.endNode
                    direction = -1;
                }
            }
            if(newRoad != null) {
                path.RemoveAt(path.Count - 1);
                //Debug.Log(path.Count);
                road = newRoad;
                road.AddCar(this, direction);
                //Debug.Log(newRoad.length);
            }
        }
    }
}
