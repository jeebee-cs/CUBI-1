using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple camera following multiple objects
public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    public Camera mainCamera { get => _mainCamera; set => _mainCamera = value; }

    public Vector3 offsetFromObjects; //initial position
    public Vector3 offsetFromPoint; //offset from where we should look at (so we dont have our player dead center)

    public float speed = .8f;

    public List<Transform> objectsToFollow; //the camera will follow the middle point of these objects

    public bool lockX;

    protected Vector3 vel = Vector3.zero;
    void Start()
    {
        _mainCamera.transform.position = offsetFromObjects;
        _mainCamera.transform.LookAt(MiddlePoint(objectsToFollow) + offsetFromPoint);
    }
    void LateUpdate()
    {
        Vector3 target = MiddlePoint(objectsToFollow) + offsetFromPoint;
        if (lockX)
        {
            target.x = transform.position.x;
        }
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, target, ref vel, speed);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Mathf.Infinity);
        transform.position = newPosition;
    }

    Vector3 MiddlePoint(List<Transform> transforms)
    {
        float posX = 0;
        float posY = 0;
        for (int i = 0; i < transforms.Count; i++)
        {
            posX += transforms[i].position.x;
            posY += transforms[i].position.y;
        }
        float centerX = posX / transforms.Count;
        float centerY = posY / transforms.Count;
        Vector3 result = new Vector3(centerX, centerY, mainCamera.transform.position.z);
        return result;
    }
}
