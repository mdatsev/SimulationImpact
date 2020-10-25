using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
     
    public float lookSpeedH = 2f;
    public float lookSpeedV = 2f;
    public float zoomSpeed = 2f;
    public float moveSpeed = 6f;

    private float yaw = 0f;
    private float pitch = 0f;
    
    void Start() {
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void Update ()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        //drag camera around with Middle Mouse
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(-Vector3.right * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.S)) {
            transform.Translate(-Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
    }
}
