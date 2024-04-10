using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dream : NetworkBehaviour
{
    [SerializeField] float dreamPoint;
    [SerializeField] public DreamType dreamType;
    NetworkObject _networkObject;
    private void Start()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        //AkSoundEngine.PostEvent("Player_Dream_Get", this.gameObject);
        GameManager.instance.uIManager.dreamBar.value += dreamPoint;
        GameManager.instance.winLoose.winCheck(dreamPoint);

        if (dreamType == 0)
        {
            GameManager.instance.dreamCollection.DreamNCollect();
        }
        DespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        _networkObject.Despawn();
    }
}


public enum DreamType
{
    NEUTRAL,
    GOOD,
    BAD
}
