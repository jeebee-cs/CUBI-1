using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class MoveableBlock : ABlock
{
    public bool IsMoving = false;
    private Vector3 directionToPush;
    [SerializeField] LayerMask _layerCollision;
    [SerializeField] LayerMask _layerBlock;
    [SerializeField] bool _bigBlock;
    public bool bigBlock { get => _bigBlock; }

    [SerializeField, Range(0f, 1f)]
    private float deplacementTime = 0.5f;
    NetworkObject _networkObject;
    public NetworkObject networkObject { get => _networkObject; }

    void Start()
    {
        _networkObject = transform.parent.GetComponent<NetworkObject>();
        if (_networkObject == null) _networkObject = GetComponent<NetworkObject>();
    }

    public void MoveBlock(Vector3 otherPos, ulong clientId)
    {
        ChangeOwnerShipServerRpc(clientId);

        Vector3 directionToOther = otherPos - transform.position;

        if (_bigBlock)
        {
            if (Mathf.Abs(directionToOther.y) > 1) return;
        }
        else if (Mathf.Abs(directionToOther.y) > .5f) return;

        directionToOther = directionToOther.normalized;

        directionToOther.x = Mathf.Round(directionToOther.x * .6f);
        directionToOther.z = Mathf.Round(directionToOther.z * .6f);
        directionToOther.y = 0;

        directionToPush = directionToOther * -1;
        if (_bigBlock)
        {
            if (directionToPush.x == 0)
            {
                if (Physics.OverlapBox(transform.position + directionToPush * 1.5f, new Vector3(.9f, .9f, .4f), Quaternion.identity, _layerBlock).Length != 0)
                {
                    Debug.Log("Block hit");
                    return;
                }
            }
            else if (directionToPush.z == 0)
            {
                if (Physics.OverlapBox(transform.position + directionToPush * 1.5f, new Vector3(.4f, .9f, .9f), Quaternion.identity, _layerBlock).Length != 0)
                {
                    Debug.Log("Block hit");
                    return;
                }
            }
        }
        else
        {
            if (Physics.OverlapBox(transform.position + directionToPush, new Vector3(.4f, .4f, .4f), Quaternion.identity, _layerBlock).Length != 0)
            {
                Debug.Log("Block hit");
                return;
            }
        }
        StartCoroutine(SmoothLerp(transform.position + directionToPush, deplacementTime));
        AkSoundEngine.PostEvent("Block_Push", gameObject);
        IsMoving = true;

    }

    private IEnumerator SmoothLerp(Vector3 newPosition, float time)
    {
        Vector3 startingPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;
        IsMoving = false;

        if(!bigBlock) GameManager.instance.winLoose.FirstBlockChange(this, startingPos);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerShipServerRpc(ulong ownerClientId)
    {
        _networkObject.ChangeOwnership(ownerClientId);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        _networkObject.Despawn();
    }
}