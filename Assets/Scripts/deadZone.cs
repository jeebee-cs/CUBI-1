using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        GameManager.instance.winLoose.Reload(1);
    }
}