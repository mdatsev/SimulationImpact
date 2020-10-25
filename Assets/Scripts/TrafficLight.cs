using System.Collections;
using System.Collections.Generic;
using System;

public class TrafficLight
{
    public Node node;
    public float accTime; //accumulated time
    public float changeTime;


    public TrafficLight(Edge n) {
        changeTime = 25; //+ (new Random().Next(10, 40));
        road = n;
        road.IsRedTrafficLight = true;
    }

    public void update(float deltaTime) {
        accTime += deltaTime;
        if (accTime > changeTime) {
            accTime -= changeTime;
            road.IsRedTrafficLight = !road.IsRedTrafficLight;
        }
    }

    public bool getStatus() {
        return road.IsRedTrafficLight;
    }

}
