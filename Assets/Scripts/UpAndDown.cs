using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    Vector2 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(UpAndDownCoroutine());
    }
    IEnumerator UpAndDownCoroutine()
    {
        while (true)
        {
            while (transform.position.y < originalPosition.y + .05f)
            {
                transform.position = transform.position + new Vector3(0, 0.001f, 0);
                yield return null;
            }
            while (transform.position.y > originalPosition.y - .05f)
            {
                transform.position = transform.position + new Vector3(0, -0.001f, 0);
                yield return null;
            }
            yield return null;
        }
    }
}
