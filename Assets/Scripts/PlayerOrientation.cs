using System.Collections;
using System.Collections.Generic;
using static DrawArrow;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    public Camera cam;
    private Vector3 viewDir;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    void Update()
    {
        viewDir = transform.position - new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        transform.forward = viewDir.normalized;
    }
}
