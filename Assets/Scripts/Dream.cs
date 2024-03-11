using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    [SerializeField] float dreamPoint;

    void OnTriggerEnter(Collider other)
    {
        GameManager.instance.uIManager.dreamBar.value += dreamPoint;
        gameObject.SetActive(false);
    }
}
