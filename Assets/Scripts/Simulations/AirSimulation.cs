using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Simulations
{
    public class AirSimulation
    {
        public int width;
        public int height;
        public List<double> grid;
        public AirSimulation() {
            width = 0;
        }
        public AirSimulation(int width_, int height_) {
            width = width_;
            height = height_;
            grid = new List<double>(new double[width * height]); 
        }

        static int mod(int n, int m) {
           return ((n % m) + m) % m;
        }

        public double get(List<double> grid_, int x, int y) {
            return grid_[getIdx(x, y)];
        }

        public int getIdx(int x, int y) {
            return mod(x, width) + width * mod(y, height);
        }

        public void Step() {
            var lastGrid = new List<double>(grid);
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    double val = get(lastGrid, x, y);
                    double basePortion = 0;
                    var neighs = new List<(int, int)> {
                        ( 1,  0), ( 1,  1), ( 0,  1), (-1,  1),
                        (-1,  0), (-1, -1), ( 0, -1), ( 1, -1)
                    };
                    var neighVals = neighs.Select(c => get(lastGrid, x + c.Item1, y + c.Item2)).ToList();
                    var dist = neighs.Select(c => Math.Sqrt(c.Item1 * c.Item1 + c.Item2 * c.Item2)).ToList();
                    for (int k = 0; k < neighs.Count; k++) {
                        if(val > get(lastGrid, x + neighs[k].Item1, y + neighs[k].Item2)) {
                            basePortion += dist[k];
                        }
                    }
                    for(int k = 0; k < neighs.Count; k++) {
                        int idx = getIdx(x + neighs[k].Item1, y + neighs[k].Item2);
                        if(val > neighVals[k])
                        {
                            double amount = ((val - lastGrid[idx]) / basePortion / dist[k]);
                            grid[idx] += amount;
                            grid[x + width * y] -= amount;
                        }
                    }
                }
            }
        }
    }
}