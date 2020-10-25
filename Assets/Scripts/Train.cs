
/*using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using UnityEngine;
namespace Simulations {
public class Train : MonoBehaviour
{
    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;
    private Simulation sim = new SimulationImpact();



    void Start()
    {
        Debug.Log("Starting");
        Application.runInBackground = true;

        sim.Init(new List<Car>(), new Map(), null);
        
        startServer();
    }

    void startServer()
    {
        SocketThread = new System.Threading.Thread(networkCode);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    private string getIPAddress()
    {
        IPHostEntry host;
        string localIP = "127.0.0.1";
        host = Dns.GetHostEntry(Dns.GetHostName());
        // foreach (IPAddress ip in host.AddressList)
        // {
        //     if (ip.AddressFamily == AddressFamily.InterNetwork)
        //     {
        //         localIP = ip.ToString();
        //     }

        // }
        return localIP;
    }


    Socket listener;
    Socket handler;

    void networkCode()
    {
        string data = null;
        byte[] bytes = new byte[1024];
        Debug.Log("Ip " + getIPAddress().ToString());
        IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
        IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 34343);
        listener = new Socket(ipArray[0].AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            while (true)
            {
                keepReading = true;
                handler = listener.Accept();
                data = null;

                while (keepReading)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    Debug.Log("Getting batch.");
                    handler.NoDelay = true;
                    int reward = 1;
                    handler.Send(Encoding.ASCII.GetBytes(String.Format("{0}\n", reward)));
                    
                    List<float> result = sim.getPoints(0,0);
                    Debug.Log(result.Count);
                    for(int row = 0; row < result.Count; row++) {
                        handler.Send(Encoding.ASCII.GetBytes(String.Format("{0}\n", result[row])));
                    }
                    handler.Send(Encoding.ASCII.GetBytes("!"));
                    handler.Close();
                    break;
                    if (bytesRec <= 0)
                    {
                        keepReading = false;
                        handler.Disconnect(true);
                        break;
                    }

                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(1); // 1ms
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        if (data != null) {
            Debug.Log("FINAL DATA " + data);
        }
    }

    void stopServer()
    {
        keepReading = false;
        if (SocketThread != null)
        {
            SocketThread.Abort();
        }

        if (handler != null && handler.Connected)
        {
            handler.Disconnect(false);
            Debug.Log("Disconnected!");
        }
    }

    void OnDisable()
    {
        stopServer();
    }
}
}*/