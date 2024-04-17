using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    Vector2 originalPosition;
    float randomSpeed;
    void Awake()
    {
        originalPosition = transform.position;
        StartCoroutine(UpAndDownCoroutine());
        randomSpeed = Random.Range(0.0007f, 0.001f);
    }
    IEnumerator UpAndDownCoroutine()
    {
        while (true)
        {
            while (transform.position.y < originalPosition.y + .05f)
            {
                transform.position = transform.position + new Vector3(0, randomSpeed, 0);
                yield return new WaitForFixedUpdate();
            }
            while (transform.position.y > originalPosition.y - .05f)
            {
                transform.position = transform.position + new Vector3(0, -randomSpeed, 0);
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }
}
