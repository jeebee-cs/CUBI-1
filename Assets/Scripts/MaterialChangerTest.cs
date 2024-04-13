using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChangerTest : MonoBehaviour
{
    [Range(0, 1)]
    public float blend;

    public GameObject go;

    public void OnValidate()
    {
        if (go != null)
            UpdateShader();
    }

    public void UpdateShader()
    {
            foreach (Material m in GetMaterialList())
            {
            if(m!=null)
                m.SetFloat("_LockedBlend", blend);
            }
    }

    public List<Material> GetMaterialList()
    {
        return go.GetComponent<Renderer>().sharedMaterials.ToList();
    }
}
