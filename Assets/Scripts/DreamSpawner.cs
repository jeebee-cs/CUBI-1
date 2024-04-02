using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamSpawner : MonoBehaviour
{
    public List<GameObject> dreamSpawnList = new List<GameObject>();
    public bool isRandom;

    void Start()
    {
        int index = isRandom ? Random.Range(-1, dreamSpawnList.Count) : 0;
        if ( index > -1 && dreamSpawnList.Count > 0)
        {
            Instantiate(dreamSpawnList[index], transform.position, Quaternion.identity);
        }
    }
}
