using System.Collections;
using System.Collections.Generic;
using System;

public class TrafficLight
{
    public Edge road;
    public int framesToChange;


    public TrafficLight(Edge n) {
        framesToChange = (30 * 60); //+ (new Random().Next(10, 40));
        road = n;
        road.IsRedTrafficLight = true;
    }

    public void update(int frames) {
        if (frames % framesToChange == 0) {
            road.IsRedTrafficLight = !road.IsRedTrafficLight;
        }
    }

    public bool getStatus() {
        return road.IsRedTrafficLight;
    }

}
