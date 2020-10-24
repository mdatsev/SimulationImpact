using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationImpact : Simulation
    {
        public override void Init(List<Car> cars) {
            this.cars = cars;
            return;
        }
        public override void Step() {
            return;
        }
    }
}
