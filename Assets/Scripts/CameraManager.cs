using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] Camera _playerCamera;
    public Camera playerCamera { get => _playerCamera; }

    public void FollowPlayer(Transform transform)
    {
        _cinemachineFreeLook.Follow = transform;
        _cinemachineFreeLook.LookAt = transform;
    }

}
