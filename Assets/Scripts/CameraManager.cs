using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    public Camera mainCamera { get => _mainCamera; set => _mainCamera = value; }
}
