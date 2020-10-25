using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public class SimulationDummy : Simulation
    {
        public override void Init(List<Car> cars, Map map, TrafficLight tf) {
            this.cars = cars;
            this.map = map;
            return;
        }
        public override void Step(float deltaTime) {
            foreach(Car car in this.cars) {
                car.Move();
            }

            return;
        }
    }
}
