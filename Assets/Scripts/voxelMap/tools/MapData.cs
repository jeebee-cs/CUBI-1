using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "NewMapData", menuName = "Custom/MapData")]
public class MapData : SerializedScriptableObject
{
    public int width;
    public int height;
    public int depth;

    public List<GameObject> blockPrefabs = new List<GameObject>();

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

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < depth; k++)
                    {
                        if (layers[i].LabledTable[j, k] != 0)
                        {
                            Vector3 cubePos = new Vector3(j-1, i-1, -k+height-2); //* layers[i].LabledTable[j, k];
                            Vector3 cubeSize = Vector3.one;// * layers[i].LabledTable[j, k];
                            Quaternion cubeRotation = Quaternion.Euler(-90, 0, 0);
                            GameObject cube = blockPrefabs[layers[i].LabledTable[j, k] - 1];
                            blockDataList.Add(new voxelMap.ABlockData(cubePos, cubeRotation, cubeSize, cube, true));
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
            layers[i] = new MapDataLayer(width, height, blockPrefabs);
        }
        LoadBlockPrefabs();
    }

    private void LoadBlockPrefabs()
    {
        // Clear existing prefabs
        blockPrefabs.Clear();

        // Load prefabs from Assets/Prefabs/Blocks
        string[] prefabPaths = Directory.GetFiles("Assets/Prefab/Blocs", "*.prefab", SearchOption.AllDirectories);
        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
                blockPrefabs.Add(prefab);
        }

        // Add a default prefab if no other prefabs are found
        if (blockPrefabs.Count == 0)
        {
            Debug.LogWarning("No block prefabs found in the directory Assets/Prefabs/Blocks. Adding a default prefab.");
        }
    }

    [Button(ButtonSizes.Medium)]
    private void CreateAsset()
    {
        string assetPath = "Assets/MapData.asset";
        AssetDatabase.CreateAsset(this, assetPath);
        AssetDatabase.SaveAssets();
        Debug.Log("MapData asset created at path: " + assetPath);
    }

    private void OnValidate()
    {
        // Ensure layers are initialized when values change in the Inspector
        if (layers == null || layers.Length != depth)
        {
            layers = new MapDataLayer[depth];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new MapDataLayer(width, height, blockPrefabs);
            }
        }
    }
}

[System.Serializable]
public class MapDataLayer
{
    [TableMatrix(DrawElementMethod = "DrawCell", SquareCells = true)]
    public int[,] LabledTable;
    private List<GameObject> blockPrefabs;

    private int DrawCell(Rect rect, int value = 0)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = (value + 1) % (blockPrefabs.Count + 1);
            GUI.changed = true;
            Event.current.Use();
        }

        EditorGUI.DrawRect(rect.Padding(1), new Color(0.1f, 0.0f, 0.2f));

        if (value != 0){
            Object cubeObject = blockPrefabs[value - 1] as Object;
            var preview = AssetPreview.GetAssetPreview(cubeObject);
            var transparentPreview = MakeGrayBackgroundTransparent(preview);
            if (preview != null)
                GUI.DrawTexture(rect, transparentPreview);
        }

        return value;
    }

    private static Texture2D MakeGrayBackgroundTransparent(Texture2D preview)
    {
        Color32[] pixels = preview.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r == pixels[i].g && pixels[i].g == pixels[i].b)
            {
                pixels[i].a = 0; //rend les pixels gris transparents
            }
        }

        Texture2D transparentPreview = new Texture2D(preview.width, preview.height);
        transparentPreview.SetPixels32(pixels);
        transparentPreview.Apply();

        return transparentPreview;
    }

    public MapDataLayer(int width, int height, List<GameObject> blockPrefabs)
    {
        LabledTable = new int[width, height];
        this.blockPrefabs = blockPrefabs;
    }
}
#endif
