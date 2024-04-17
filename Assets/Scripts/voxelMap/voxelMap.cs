using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static ABlock;

public class voxelMap : MonoBehaviour {
    private ABlock[,,] voxelMatrix; // Matrice 3D de blocs

    [SerializeField ,Range(0,20)]
    private int width = 5;

    [SerializeField ,Range(0,20)]
    private int height = 5;

    [SerializeField ,Range(0,20)]
    private int depth = 5;
    Vector2 _firstPosBlock = new Vector2(int.MaxValue, int.MaxValue);
    public Vector2 firstPosBlock {get => _firstPosBlock;}
    Vector2 _firstBlockOriginalPos = new Vector2(int.MaxValue, int.MaxValue);
    public Vector2 firstBlockOriginalPos {get => _firstBlockOriginalPos; set => _firstBlockOriginalPos = value; }
    Vector2 _firstBlockOriginalPosThisGame = new Vector2(int.MaxValue, int.MaxValue);
    public Vector2 firstBlockOriginalPosThisGame {get => _firstBlockOriginalPosThisGame; set => _firstBlockOriginalPosThisGame = value; }
    Vector2 _firstBlockPosThisGame = new Vector2(int.MaxValue, int.MaxValue);
    public Vector2 firstBlockPosThisGame {get => _firstBlockPosThisGame; set => _firstBlockPosThisGame = value; }

    private Vector3 offset;

    public MapTheme mapTheme;

    [SerializeField] private BlockTheme jungleTheme;
    [SerializeField] private BlockTheme clockTheme;
    [SerializeField] private BlockTheme pastelTheme;

    //PlayerDetection
    BoxCollider col;

    void Awake()
    {
        offset = transform.position;
        voxelMatrix = new ABlock[width,height,depth];
        //childsToMatrix();
        if (GetComponent<BoxCollider>() == null) { }
            gameObject.AddComponent<BoxCollider>();
        col = GetComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(width* 2, height, depth*2);
        col.center = (new Vector3(width, height, depth) / 2);
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

                blockDataList.Add(new ABlockData(relativePosition, Quaternion.Euler(90, 0, 0), child.localScale, isMovable));
            }
        }

        string json = JsonUtility.ToJson(new MapData(width, height, depth, mapTheme, blockDataList.ToArray()));

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

            MapTheme currentTheme = mapData.mapTheme;

            bool isBig = (blockData.scale == Vector3.one * 2) ? true : false;

            GameObject prefab = RandomPrefabFromTheme(currentTheme, isBig);

            if (prefab != null)
            {
                // Place the prefab in the scene at the specified position
                GameObject blockObject;
                #if UNITY_EDITOR
                    blockObject = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
                #else
                    blockObject = Instantiate(prefab, transform.position, Quaternion.identity);
                #endif
                blockObject.transform.position = absolutePosition;
                blockObject.transform.rotation = Quaternion.Euler(-90,(isBig?-1:Random.Range(0,3))*90,0);
            }
            else
            {
                Debug.LogWarning("Prefab is null for block data: " + blockData.ToString());
            }
        }
    }

    public GameObject RandomPrefabFromTheme(MapTheme theme, bool isBig)
    {
        switch (theme)
        {
            case MapTheme.JUNGLE:
                return (isBig)? jungleTheme.bigBlocks[Random.Range(0, jungleTheme.bigBlocks.Count)] : jungleTheme.smallBlocks[Random.Range(0, jungleTheme.smallBlocks.Count)];
            case MapTheme.CLOCK:
                return (isBig) ? clockTheme.bigBlocks[Random.Range(0, clockTheme.bigBlocks.Count)] : clockTheme.smallBlocks[Random.Range(0, clockTheme.smallBlocks.Count)];
            case MapTheme.PASTEL:
                return (isBig) ? pastelTheme.bigBlocks[Random.Range(0, pastelTheme.bigBlocks.Count)] : pastelTheme.smallBlocks[Random.Range(0, pastelTheme.smallBlocks.Count)];
            default:
                return (isBig) ? jungleTheme.bigBlocks[Random.Range(0, jungleTheme.bigBlocks.Count)] : jungleTheme.smallBlocks[Random.Range(0, jungleTheme.smallBlocks.Count)];
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            GameManager.instance.skyboxBlender.ChangeSkyboxTheme(mapTheme);
        }
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
        public MapTheme mapTheme;
        public ABlockData[] blockDataArray;

        public MapData(int width, int height, int depth, MapTheme theme,ABlockData[] blockDataArray)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.mapTheme = theme;
            this.blockDataArray = blockDataArray;
        }
    }

    [System.Serializable]
    public class ABlockData
    {
        public Vector3 position;
        public Vector3 scale;

        public Quaternion rotation;
        public bool isMovable;

        public ABlockData(Vector3 position, Quaternion rotation, Vector3 scale, bool isMovable)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.isMovable = isMovable;
        }
    }
}