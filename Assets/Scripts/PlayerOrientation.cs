using System.Collections;
using System.Collections.Generic;
using static DrawArrow;
using UnityEngine;
public class PlayerOrientation : MonoBehaviour
{
    public Transform CameraT;
    private Vector3 viewDir;
    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        viewDir = transform.position - new Vector3(CameraT.position.x, transform.position.y, CameraT.position.z);
        transform.forward = viewDir.normalized;
        transform.forward = new Vector3(Vector3.Dot(CameraT.forward, new Vector3(1,0,0)),0,Vector3.Dot(CameraT.forward, new Vector3(0,0,1)));
        transform.forward.Normalize();
    }
}