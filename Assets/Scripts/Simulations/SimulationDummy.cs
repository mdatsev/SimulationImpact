using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationDummy : Simulation
    {
        public override void Init(List<Car> cars, TrafficLight tf) {
            this.cars = cars;
            return;
        }
        public override void Step(int frames) {
            foreach(Car car in this.cars) {
                car.Move();
            }

            return;
        }
    }
}
