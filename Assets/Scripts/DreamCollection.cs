using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DreamCollection : MonoBehaviour
{

    public void DreamNCollect()
    {
        if (GameManager.instance.neutralDreamCollected <= 6)
        {
            GameManager.instance.SetNeutralDreamCollectedServerRpc(GameManager.instance.neutralDreamCollected + 1);
        }
    }
}