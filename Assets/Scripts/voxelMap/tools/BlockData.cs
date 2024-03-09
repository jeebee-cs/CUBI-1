using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Custom/BlockData")]
public class BlockData : ScriptableObject
{
    public Vector3 position;
    public Vector3 scale;
    public GameObject prefab; // Reference to the prefab
    public bool isMovable;
}
