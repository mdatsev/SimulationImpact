using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationDummy : Simulation
    {
        public override void Init(List<Car> cars) {
            this.cars = cars;
            return;
        }
        public override void Step() {
            foreach(Car car in cars) {
                car.Move();
            }

            return;
        }
    }
}
