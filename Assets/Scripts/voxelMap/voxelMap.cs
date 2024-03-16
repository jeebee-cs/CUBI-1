using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static ABlock;

public class voxelMap : MonoBehaviour {
    private ABlock[,,] voxelMatrix; // Matrice 3D de blocs

    [SerializeField ,Range(0,11)]
    private int width = 5;

    [SerializeField ,Range(0,11)]
    private int height = 5;

    [SerializeField ,Range(0,11)]
    private int depth = 5;

    private Vector3 offset;

    void Awake()
    {
        offset = transform.position;
        voxelMatrix = new ABlock[width,height,depth];
        childsToMatrix();
    }

    // Méthode pour ajouter un bloc à la carte de voxels
    public void AddBlock(ABlock block)
    {
        Vector3 blockPos = block.Position - offset;
        voxelMatrix[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = block;
        block.cube.transform.parent = transform;
    }

#if UNITY_EDITOR
    // Fonction pour sauvegarder la map dans un fichier JSON
    public void SaveMapToFile(string filePath)
    {
        List<ABlockData> blockDataList = new List<ABlockData>();

        foreach (Transform child in transform)
        {
            ABlock blockComponent = child.GetComponent<ABlock>();
            if (blockComponent != null)
            {
                bool isMovable = blockComponent is MoveableBlock;
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);

                // Calculate the relative position of the block
                Vector3 relativePosition = child.position - transform.position;
                Debug.Log(relativePosition);

                blockDataList.Add(new ABlockData(relativePosition, child.rotation, child.localScale, prefab, isMovable));
            }
        }

        string json = JsonUtility.ToJson(new MapData(width, height, depth, blockDataList.ToArray()));

        File.WriteAllText(filePath, json);

        Debug.Log("Voxel map saved to file: " + filePath);
    }
#endif

    public void LoadMapFromFile(string filePath)
    {
        offset = transform.position;
        string json = File.ReadAllText(filePath);

        MapData mapData = JsonUtility.FromJson<MapData>(json);

        width = mapData.width;
        height = mapData.height;
        depth = mapData.depth;

        ClearBlocks();

        foreach (ABlockData blockData in mapData.blockDataArray)
        {
            Vector3 absolutePosition = blockData.position + offset + new Vector3(1.5f,1.5f,1.5f);

            GameObject prefab = blockData.prefab;

            if (prefab != null)
            {
                // Place the prefab in the scene at the specified position
                #if UNITY_EDITOR
                GameObject blockObject = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
                #else
                GameObject blockObject = Instantiate(prefab, transform.position, Quaternion.identity);
                #endif
                blockObject.transform.position = absolutePosition;
                blockObject.transform.rotation = blockData.rotation;
            }
            else
            {
                Debug.LogWarning("Prefab is null for block data: " + blockData.ToString());
            }
        }
    }

    private void childsToMatrix()
    {
        foreach (Transform child in transform)
        {
            Vector3 inMatrixPos = child.position - offset;

            if(IsWithinBounds(inMatrixPos))
            {
                ABlock blockComponent = child.GetComponent<ABlock>();

                if (blockComponent == null)
                {
                    continue;
                }

                blockComponent.Position = child.position;
                blockComponent.Mesh = child.GetComponent<MeshFilter>().sharedMesh;
                blockComponent.Material = child.GetComponent<MeshRenderer>().sharedMaterial;
                Debug.Log(inMatrixPos);

                voxelMatrix[(int)Mathf.Ceil(inMatrixPos.x), (int)Mathf.Ceil(inMatrixPos.y), (int)Mathf.Ceil(inMatrixPos.z)] = blockComponent;
            }
            else Destroy(child.gameObject);
        }
    }
    private void ClearBlocks()
    {
        foreach (Transform child in transform)
        {
        #if UNITY_EDITOR
            EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
        #else
            Destroy(child.gameObject);
        #endif
        }
    }

    private bool IsWithinBounds(Vector3 position)
    {
        Vector3 adjustedPosition = position ;

        return adjustedPosition.x >= 0 && adjustedPosition.x < width &&
            adjustedPosition.y >= 0 && adjustedPosition.y < height &&
            adjustedPosition.z >= 0 && adjustedPosition.z < depth;
    }

    private void OnDrawGizmos()
    {
       float voxelSize = 1f;

        // Calculer l'origine de la boîte de gizmo
        Vector3 origin = transform.position;

        // Dessiner une boîte autour de la matrice 3D
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(origin + new Vector3(width, height, depth) * voxelSize * 0.5f, new Vector3(width, height, depth) * voxelSize);
    }


    //classes serializable afin de stocker les maps en format json
    [System.Serializable]
    public class MapData
    {
        public int width;
        public int height;
        public int depth;
        public ABlockData[] blockDataArray;

        public MapData(int width, int height, int depth, ABlockData[] blockDataArray)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.blockDataArray = blockDataArray;
        }
    }

    [System.Serializable]
    public class ABlockData
    {
        public Vector3 position;
        public Vector3 scale;

        public Quaternion rotation;
        public GameObject prefab; // Reference to the prefab
        public bool isMovable;

        public ABlockData(Vector3 position, Quaternion rotation, Vector3 scale, GameObject prefab, bool isMovable)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.prefab = prefab;
            this.isMovable = isMovable;
        }
    }
}