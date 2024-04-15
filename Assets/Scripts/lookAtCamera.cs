using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Assuming the main camera is tagged as "MainCamera"
        mainCameraTransform = GameManager.instance.cameraManager.playerCamera.transform;
    }

    void Update()
    {
        // Calculate the direction to the camera
        Vector3 directionToCamera = mainCameraTransform.position - transform.position;

        // Make the object face the camera (only rotate around the Y-axis)
        directionToCamera.y = 0; // Set y component to 0 to make it level with the ground
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

        // Apply the rotation to the object
        transform.rotation = targetRotation;
    }
}

