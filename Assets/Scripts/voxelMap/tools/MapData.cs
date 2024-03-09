using UnityEngine;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Custom/MapData")]
public class MapData : ScriptableObject
{
    public int width;
    public int height;
    public int depth;
    public BlockData[] blockDataArray;
}