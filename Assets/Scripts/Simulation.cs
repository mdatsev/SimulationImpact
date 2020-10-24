using System;

namespace Simulations
{
    public abstract class Simulation
    {
        public abstract void Init();
        public abstract void Step();
    }
}