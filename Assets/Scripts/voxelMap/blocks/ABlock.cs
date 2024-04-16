using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class ABlock : NetworkBehaviour
{
    // Propriétés communes à tous les blocs
    public Vector3 Position { get; set; }

    public Mesh Mesh { get; set; }
    public Material Material { get; set; }

    public GameObject cube { get; set;}
    [SerializeField] voxelMap _voxelMap;
    public voxelMap voxelMap { get => _voxelMap; set => _voxelMap = value; }

    protected virtual void OnValidate()
    {
        // Update the mesh and material properties if they are null
        if (Mesh == null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh = meshFilter.sharedMesh;
            }
        }

        if (Material == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Material = renderer.sharedMaterial;
            }
        }
    }
    public void Render()
    {
        cube = new GameObject("Block");
        cube.transform.position = Position;

        MeshFilter meshFilter = cube.AddComponent<MeshFilter>();
        meshFilter.mesh = Mesh;

        Renderer renderer = cube.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = Material;
    }

}
