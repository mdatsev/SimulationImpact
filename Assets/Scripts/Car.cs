using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car : MonoBehaviour
    {
        public float velocity = 1;

        private float position = 0;
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
            float n = (position) / road.length;
        
            transform.position = (road.endNode.position - road.startNode.position) * n;
        }
        public void changeRoad(Edge newRoad) {
            position = position - road.length;
            this.road = newRoad;

        }
    }
}
