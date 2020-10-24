using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations {
    public class Car : MonoBehaviour
    {
        public float speed = 1;

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
            float n = (float)((position + speed) / road.lenght);
        
            transform.position = (road.endNode.position - road.startNode.position) * n;
        }

        public void changeRoad(Edge newRoad) {
            this.road = newRoad;

            position = 0;
        }
    }
}
