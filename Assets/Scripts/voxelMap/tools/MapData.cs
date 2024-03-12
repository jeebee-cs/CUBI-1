using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Custom/MapData")]
public class MapData : SerializedScriptableObject
{
    public int width;
    public int height;
    public int depth;

    public GameObject cube;

    public voxelMap voxelMap;

    [ShowInInspector, HideReferenceObjectPicker]
    private MapDataLayer[] layers;

    [Button(ButtonSizes.Medium)]
    private void SaveTable()
    {
        string filePath = EditorUtility.SaveFilePanel("Save Voxel Map", "", "VoxelMap", "json");
        if (!string.IsNullOrEmpty(filePath))
        {
            List<voxelMap.ABlockData> blockDataList = new List<voxelMap.ABlockData>();

            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    for (int k = 0; k < depth; k++)
                    {
                        if (layers[i].LabledTable[j, k] != 0)
                        {
                            Vector3 cubePos = new Vector3(i-1, j-1, k-1) * layers[i].LabledTable[j, k];
                            Vector3 cubeSize = Vector3.one * layers[i].LabledTable[j, k];
                            blockDataList.Add(new voxelMap.ABlockData(cubePos, cubeSize, cube, true));
                        }
                    }
                }
            }
            string json = JsonUtility.ToJson(new voxelMap.MapData(width, height, depth, blockDataList.ToArray()));

            File.WriteAllText(filePath, json);

            Debug.Log("Voxel map saved to file: " + filePath);
        }
    }

    [Button(ButtonSizes.Medium)]
    private void UpdateTable()
    {
        layers = new MapDataLayer[depth];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new MapDataLayer(width, height);
        }
    }
}

[System.Serializable]
public class MapDataLayer
{
    [TableMatrix(DrawElementMethod = "DrawCell",SquareCells =true)]
    public int[,] LabledTable;

    private static int DrawCell(Rect rect, int value)
    {
        if(Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = (value + 1) % 3;
            GUI.changed = true;
            Event.current.Use();
        }

        EditorGUI.DrawRect(rect.Padding(1), new Color(0.1f, 0.2f * value, 0.2f));

        return value;
    }
    public MapDataLayer(int width, int height)
    {
        LabledTable = new int[width, height];
    }
}