using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTheme", menuName = "Custom/Theme")]
public class BlockTheme : ScriptableObject
{
    public List<GameObject> smallBlocks;

    public List<GameObject> bigBlocks;
}
