using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.transform.position = collision.gameObject.GetComponent<PushBlock>().lastGroundPosition;
        collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
    }
}