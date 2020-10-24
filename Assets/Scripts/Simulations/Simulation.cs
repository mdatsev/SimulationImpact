using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulations
{
    public abstract class Simulation
    {
        public List<Car> cars;

        public abstract void Init(List<Car> cars);
        public abstract void Step();
    }
}