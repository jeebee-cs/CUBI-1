using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dream : NetworkBehaviour
{
    [SerializeField] float dreamPoint;
    [SerializeField] private DreamType dreamType;
    [SerializeField] private Material goodDreamMat;
    [SerializeField] private Material badDreamMat;
    [SerializeField] private Material neutralDreamMat;

    NetworkObject _networkObject;
    private void Start()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AI")
        {
            AkSoundEngine.PostEvent("AI_Dream_Get", this.gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Player_Dream_Get", this.gameObject);
        }
        GameManager.instance.uIManager.dreamBar.value += dreamPoint;
        GameManager.instance.winLoose.winCheck(dreamPoint);

        if (dreamType == DreamType.NEUTRAL)
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

    public void setDreamType(DreamType newDreamType)
    {
        Material[] materialList = {neutralDreamMat, goodDreamMat, badDreamMat};
        dreamType = newDreamType;
        gameObject.GetComponent<Renderer>().material = materialList[(int)newDreamType];
    }

    public DreamType GetDreamType()
    {
        return dreamType;
    }
}


public enum DreamType
{
    NEUTRAL,
    GOOD,
    BAD
}
