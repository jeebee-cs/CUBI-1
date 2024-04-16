using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dream : NetworkBehaviour
{
    [SerializeField] float dreamPointGood = 0.5f;
    [SerializeField] float dreamPointBad = -0.5f;
    [SerializeField] float dreamPointNeutral = 0.1f;
    [SerializeField] private DreamType dreamType;
    [SerializeField] private RuntimeAnimatorController  goodDreamAnim;
    [SerializeField] private RuntimeAnimatorController  badDreamAnim;
    [SerializeField] private RuntimeAnimatorController  neutralDreamAnim;
    NetworkObject _networkObject;
    private void Start()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AI")
        {
            //AkSoundEngine.PostEvent("AI_Dream_Get", this.gameObject);
        }
        else
        {
            //AkSoundEngine.PostEvent("Player_Dream_Get", this.gameObject);
        }
        float[] dreamTypeList = {dreamPointNeutral, dreamPointGood, dreamPointBad};

        GameManager.instance.SetDreamEnergyServerRpc(GameManager.instance.dreamEnergy + dreamTypeList[(int)dreamType]);
        GameManager.instance.winLoose.winCheck();

        if (dreamType == DreamType.NEUTRAL)
        {
            GameManager.instance.dreamCollection.DreamNCollect();
        }

        //Send dreamType to DreamDisplayer (handles dialogues)
        GameManager.instance.dreamDisplayer.DisplayDreamDialogue(dreamType);

        DespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        _networkObject.Despawn();
    }

    public void setDreamType(DreamType newDreamType)
    {
        RuntimeAnimatorController[] animList = {neutralDreamAnim, goodDreamAnim, badDreamAnim};
        dreamType = newDreamType;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = animList[(int)newDreamType];
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
