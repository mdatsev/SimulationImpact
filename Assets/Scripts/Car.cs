using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car : MonoBehaviour
    {
        public double velocity = 1;
        public double position = 0;
        private int lane = 0;
        private Edge road;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Move() {

            position += velocity;
            double n = (position) / road.length;
            transform.position = (road.endNode.position - road.startNode.position) * (float)n;
        }
        public void changeRoad(Edge newRoad) {
            if (road != null) {
                position = position - road.length;
            }
            this.road = newRoad;
        }
    }
}
