using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulations
{
    public class SimulationImpact : Simulation
    {
        const double b = 0.05; // decel rate
        const double deltaT = 0.3; // reaction time / timestep
        const double S0 = 3.5; // min gap
        const double a = 0.05; // acceleration
        const double V0 = 3; // desired speed

        public override void Init(List<Car> cars, TrafficLight tf) {
            this.tf = tf;
            this.cars = cars;
            return;
        }
        public override void Step(int frames) {
            foreach(Car c in this.cars) {
                Car LV = getCarInfront(c); // leading vehicle
                if (LV == null && c.road.IsRedTrafficLight)
                {
                    Debug.Log(c.road.length);
                    Debug.Log(c.position);
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
                            LV = nextRoad.getLastCar();
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
            //tf.update(frames);
        }


        private double Vsafe(double s, double Vl) {
            /*Debug.Log(b * deltaT);
            Debug.Log(Math.Sqrt(Math.Pow(b * deltaT, 2)));
            Debug.Log(Math.Pow(Vl, 2));
            Debug.Log(2 * b * (s - S0));
            Debug.Log(Vl);
            Debug.Log(s);*/
            return -b * deltaT + Math.Sqrt(Math.Pow(b * deltaT, 2) + Math.Pow(Vl, 2) + 2 * b * (s - S0));
        }

        private double newVelocity(double Vt, bool vSafeNeeded, double s = -1, double Vl = -1) {
            const double factor = 25f;
            double adjS = vSafeNeeded ? s : factor;
            double adjA = Math.Min(a * adjS / factor, a);
            double newVt = Math.Min(Vt + adjA * deltaT, V0);
            if(vSafeNeeded){
                /*Debug.Log("NEW CALC");
                Debug.Log(Vt + a * deltaT);
                Debug.Log(Vsafe(s, Vl));
                Debug.Log(V0);*/
                newVt = Math.Min(newVt, Vsafe(s, Vl));
            }
            return newVt;
        }
    }
}
