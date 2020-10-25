
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using UnityEngine;

public class Train : MonoBehaviour
{
    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;
    
    void Start()
    {
        Debug.Log("Starting");
        Application.runInBackground = true;
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
                    Debug.Log(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    // handler.Send(bytes);
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
