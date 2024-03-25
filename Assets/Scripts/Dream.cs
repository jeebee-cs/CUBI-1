using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    [SerializeField] float dreamPoint;
    [SerializeField] int dreamType; //0 neutral, 1 positive, 2 negative

    void OnTriggerEnter(Collider other)
    {
        AkSoundEngine.PostEvent("Player_Dream_Get", this.gameObject);
        GameManager.instance.uIManager.dreamBar.value += dreamPoint;

        if (dreamType == 0)
        {
            GameManager.instance.neutralDreamCollection.DreamNCollect();
        }
        gameObject.SetActive(false);
    }
}
