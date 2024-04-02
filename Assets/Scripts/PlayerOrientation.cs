using System.Collections;
using System.Collections.Generic;
using static DrawArrow;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    public Transform Camera;
    private Vector3 viewDir;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Camera = GameManager.instance.cameraManager.mainCamera.transform;
    }

    void Update()
    {
        transform.forward = new Vector3(Vector3.Dot(Camera.forward, new Vector3(1,0,0)),0,Vector3.Dot(Camera.forward, new Vector3(0,0,1)));
        transform.forward.Normalize();
    }
}
