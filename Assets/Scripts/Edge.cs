using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulations;

public class Edge
{
    public Node startNode;
    public Node endNode;
    public bool IsRedTrafficLight;
    public int forwardLanes;
    public int backwardLanes;
    public int maxSpeed;
    public float length;
    public string name;
    public Vector3 direction;
    private List<Car> cars;


    public Car getCarInfront(Car c)
    {
        Car infront = null;
        int newCarIdx = cars.IndexOf(c) + 1;
        if (cars.Count > newCarIdx)
        {
            infront = cars[newCarIdx];
        }
        return infront;
    }
    public Car getLastCar()
    {
        Car lastCar = null;
        if(cars.Count > 0)
        {
            lastCar = cars[0];
        }
        return lastCar;
    }

    public void RemoveCar(Car c)
    {
        cars.Remove(c);
        Debug.Log(cars.Count);
    }

    public void AddCar(Car c)
    {
        cars.Add(c);
        cars.Sort(delegate (Car c1, Car c2) {
            return (int)(c1.position - c2.position);
        });
    }

    public Edge(Node n1, Node n2, int fl, int bl, int maxs, string n)
    {
        startNode = n1;
        endNode = n2;
        forwardLanes = fl;
        backwardLanes = bl;
        maxSpeed = maxs;
        name = n;
        float dx = n2.position.x - n1.position.x;
        float dz = n2.position.z - n1.position.z;
        direction = n2.position - n1.position;
        length = (float)Math.Sqrt((dx * dx) + (dz * dz));
        cars = new List<Car>();
    }

}
