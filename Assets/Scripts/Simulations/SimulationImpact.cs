using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationImpact : Simulation
    {
        const double b = 0.4;
        const double deltaT = 0.1; // reaction time / timestep
        const double S0 = 2; // min gap
        const double a = 0.1; // acceleration
        const double V0 = 120; // desired speed

        public override void Init(List<Car> cars) {
            this.cars = cars;
            return;
        }
        public override void Step() {
            foreach(Car c in this.cars) {
                Car LV = getCarInfront(c); // leading vehicle
                if(LV)
                    c.velocity = newVelocity(c.velocity, LV.position, LV.velocity);
                else
                    c.velocity = newVelocity(c.velocity);
                c.Move();
            }
        }


        private double Vsafe(double s, double Vl) {
            return -b * deltaT + Math.Sqrt(Math.Pow(b * deltaT, 2) + Math.Pow(Vl, 2) + 2 * b * (s - S0));
        }

        private double newVelocity(double Vt, double s = -1, double Vl = -1) {
            double newVt = Math.Min(Vt + a * deltaT, V0);
            if(s != -1 && Vl != -1){
                newVt = Math.Min(newVt, Vsafe(s, Vl));
            }
            return newVt;
        }
    }
}
