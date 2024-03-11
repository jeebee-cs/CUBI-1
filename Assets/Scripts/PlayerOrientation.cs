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
        viewDir = transform.position - new Vector3(Camera.position.x, transform.position.y, Camera.position.z);
        transform.forward = viewDir.normalized;
    }
}
