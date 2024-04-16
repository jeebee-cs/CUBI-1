using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DreamSpawner : MonoBehaviour
{
    public List<GameObject> dreamSpawnList = new List<GameObject>();
    public bool isRandom;

    void Start()
    {
       NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
    }

    void OnClientConnectedCallback(ulong id)
    {
        if (id == 0)
        {
            int index = isRandom ? Random.Range(-1, dreamSpawnList.Count) : 0;
            if (index > -1 && dreamSpawnList.Count > 0)
            {
                SpawnServerRpc(index);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(int index)
    {
        GameObject gameObjectDream = Instantiate(dreamSpawnList[index], transform.position, Quaternion.identity);
        gameObjectDream.GetComponent<NetworkObject>().Spawn();
    }

}
