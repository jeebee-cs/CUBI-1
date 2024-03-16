using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class voxelMapEditorWindow : EditorWindow
{
    private voxelMap voxelMap;

    private int width;
    private int height;
    private int depth;

    [MenuItem("Window/Voxel Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<voxelMapEditorWindow>("Voxel Map Editor");
    }

    private void OnGUI()
    {
        if (Selection.activeGameObject != null)
        {
            voxelMap selectedVoxelMap = Selection.activeGameObject.GetComponent<voxelMap>();
            if (selectedVoxelMap != null)
            {
                voxelMap = selectedVoxelMap;
            }
        }

        voxelMap = EditorGUILayout.ObjectField("Voxel Map", voxelMap, typeof(voxelMap), true) as voxelMap;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Map To File") && voxelMap != null)
        {
            string filePath = EditorUtility.SaveFilePanel("Save Voxel Map", "", "VoxelMap", "json");
            if (!string.IsNullOrEmpty(filePath))
            {
                voxelMap.SaveMapToFile(filePath);
            }
        }

        if (GUILayout.Button("Load Map From File") && voxelMap != null)
        {
            string filePath = EditorUtility.OpenFilePanel("Load Voxel Map", "", "json");
            if (!string.IsNullOrEmpty(filePath))
            {
                voxelMap.LoadMapFromFile(filePath);
            }
        }
    }
}
#endif