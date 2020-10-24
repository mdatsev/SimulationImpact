using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationImpact : Simulation
    {
        const float b = 0.4;
        const float deltaT = 0.1; // reaction time / timestep
        const float S0 = 2; // min gap
        const float a = 0.1; // acceleration
        const float V0 = 120; // desired speed

        public override void Init(List<Car> cars) {
            this.cars = cars;
            return;
        }
        public override void Step() {
            foreach(Car c in this.cars) {
                Car LV = getCarInfront(c).position; // leading vehicle
                c.velocity = newVelocity(car.velocity, LV.position, LV.velocity);
                c.Move();
            }
            return;
        }
        
        private float Vsafe(float s, float Vl) {
            return -b * deltaT + Math.Sqrt(Math.Pow(b * deltaT, 2) + Math.Pow(Vl, 2), 2 * b * (s - S0));
        }

        private float newVelocity(float Vt, float s, float Vl) {
            float newVt = Math.Min(Vt + a * deltaT, V0);
            newVt = Math.Min(newVt, Vsafe(s, Vl));
            return newVt;
        }
    }
}
