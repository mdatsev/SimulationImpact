using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations
{
    public class SimulationImpact : Simulation
    {
        const double b = 1; // decel rate
        const double reactionTime = 1.1; // reaction time / timestep
        const double S0 = 3; // min gap
        const double a = 1.5; // acceleration
        const double V0 = 4; // desired speed

        private float deltaTime; 

        public override void Init(List<Car> cars, Map map, TrafficLight tf) {
            this.tf = tf;
            this.map = map;
            this.cars = cars;
            return;
        }
        public override void Step(float deltaTime) {
            this.deltaTime = deltaTime;
            //Debug.Log(this.deltaTime);
            foreach(Car c in this.cars) {
                Car LV = getCarInfront(c, c.direction); // leading vehicle
               // Debug.Log(tf.canPass(c.direction == 1 ? c.road.startNode : c.road.endNode));
                if (LV == null && !tf.canPass(c.direction == 1 ? c.road.startNode: c.road.endNode))
                {
                    //Debug.Log(c.road.length);
                    //Debug.Log(c.position);
                    c.velocity = newVelocity(c.velocity, true, c.road.length - c.position, 0.0);
                }
                else
                {
                    if(LV != null)
                    {

                        c.velocity = newVelocity(c.velocity, true, LV.position - c.position, LV.velocity);
                    }
                    else {
                        // u are the leading car
                        Edge nextRoad = c.getNextRoad();
                        if (nextRoad != null)
                        {
                            LV = nextRoad.getLastCar(c.direction);
                            if (LV != null)
                            {
                                c.velocity = newVelocity(c.velocity, true, LV.position + c.road.length - c.position, LV.velocity);
                            }
                            else
                            {
                                c.velocity = newVelocity(c.velocity, false);
                            }
                        }
                        else
                        {
                            c.velocity = newVelocity(c.velocity, false);
                        }
                    
                    }

                }
                c.Move();
            }
            tf?.update(this.deltaTime);
        }


        private double Vsafe(double s, double Vl) {
            /*Debug.Log(b * reactionTime);
            Debug.Log(Math.Sqrt(Math.Pow(b * reactionTime, 2)));
            Debug.Log(Math.Pow(Vl, 2));
            Debug.Log(2 * b * (s - S0));
            Debug.Log(Vl);
            Debug.Log(s);*/
            return -b * reactionTime + Math.Sqrt(Math.Pow(b * reactionTime, 2) + Math.Pow(Vl, 2) + 2 * b * (s - S0));
        }

        private double newVelocity(double Vt, bool vSafeNeeded, double s = -1, double Vl = -1) {
            const double factor = 25f;
            double adjS = vSafeNeeded ? s : factor;
            double adjA = Math.Min(a * adjS / factor, a);
            double newVt = Math.Min(Vt + adjA * deltaTime, V0);
            //Debug.Log(newVt);
            if(vSafeNeeded){
                /*Debug.Log("NEW CALC");
                Debug.Log(Vt + a * reactionTime);
                Debug.Log(Vsafe(s, Vl));
                Debug.Log(V0);*/
                newVt = Math.Min(newVt, Vsafe(s, Vl));
            }
            return newVt;
        }
    }
}
