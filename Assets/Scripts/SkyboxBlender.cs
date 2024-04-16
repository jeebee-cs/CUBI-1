using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkyboxBlender : MonoBehaviour
{
    public float transitionTime = 2;

    public Cubemap[] skyboxes;
    public MapTheme currentTheme;
    bool changingSkybox;
    MapTheme nextTheme;

    private Cubemap skybox1;
    private Cubemap skybox2;

    public Material blendedMaterial;

    [Range(0,1)]
    public float blend;

    public bool blending = false;

    private ReflectionProbe probeComponent = null;
    public GameObject probeGameObject = null;
    private Cubemap blendedCubemap = null;
    private int renderId = -1;

    public List<ShitToRender> shitToRenderHAHAHA = new List<ShitToRender>();

    public void Start()
    {
        ChangeSkyboxTheme(MapTheme.JUNGLE);
    }

    public void ChangeSkyboxTheme(MapTheme theme)
    {
        Debug.Log("balls");
        nextTheme = theme;
        blending = true;
        for(int i = 0; i < shitToRenderHAHAHA.Count;i++)
        {
            if(i == (int)currentTheme)
            {
                foreach(GameObject go in shitToRenderHAHAHA[i].shits)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject go in shitToRenderHAHAHA[i].shits)
                {
                    go.SetActive(false);
                }
            }
        }
        skybox1 = skyboxes[(int)currentTheme];
        skybox2 = skyboxes[(int)nextTheme];
        StartCoroutine(LerpBlend(1, transitionTime));
    }
    IEnumerator LerpBlend(float endValue, float duration)
    {
        float time = 0;
        float startValue = blend = 0;

        while(time < duration)
        {
            UpdateSkybox();
            UpdateShaderParameters();
            blend = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        blending = false;
        currentTheme = nextTheme;
        blend = 0;
        yield return null;
    }
    public void UpdateSkybox()
    {
        UpdateLighting();
        UpdateReflections();
    }
    void CreateReflectionProbe()
    {
        //Search for the reflection probe object
        probeGameObject = GameObject.Find("Skybox Blender Reflection Probe");

        if (!probeGameObject)
        {
            //Create the gameobject if its not here
            probeGameObject = new GameObject("Skybox Blender Reflection Probe");
            probeGameObject.transform.parent = gameObject.transform;
            // Use a location such that the new Reflection Probe will not interfere with other Reflection Probes in the scene.
            probeGameObject.transform.position = new Vector3(0, -1000, 0);
        }

        probeComponent = probeGameObject.GetComponent<ReflectionProbe>();

        if (probeComponent)
        {
            Destroy(probeComponent);
        }

        // Create a Reflection Probe that only contains the Skybox. The Update function controls the Reflection Probe refresh.
        probeComponent = probeGameObject.AddComponent<ReflectionProbe>() as ReflectionProbe;

    }
    public void UpdateReflectionProbe()
    {
        probeComponent = probeGameObject.GetComponent<ReflectionProbe>();

        probeComponent.resolution = 128;
        probeComponent.size = new Vector3(1, 1, 1);
        probeComponent.cullingMask = 0;
        probeComponent.clearFlags = ReflectionProbeClearFlags.Skybox;
        probeComponent.mode = ReflectionProbeMode.Realtime;
        probeComponent.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
        probeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;

        // A cubemap is used as a default specular reflection.
        blendedCubemap = new Cubemap(probeComponent.resolution, probeComponent.hdr ? TextureFormat.RGBAHalf : TextureFormat.RGBA32, true);

        //Set the render reflection mode to Custom
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
        RenderSettings.customReflection = blendedCubemap;
    }
    void UpdateReflections()
    {
        if(!probeGameObject || !probeComponent)
            UpdateReflectionProbe();
        if ((SystemInfo.copyTextureSupport & CopyTextureSupport.RTToTexture) != 0)
        {
            // Wait until previous RenderProbe is finished before we refresh the Reflection Probe again.
            // renderId is a token used to figure out when the refresh of a Reflection Probe is finished. The refresh of a Reflection Probe can take mutiple frames when time-slicing is used.
            if (renderId == -1 || probeComponent.IsFinishedRendering(renderId))
            {
                if (probeComponent.IsFinishedRendering(renderId))
                {
                    //Debug.Log("probeComponent.texture.width = " + probeComponent.texture.width + " blendedCubemap.width = "+ blendedCubemap.width);
                    //Debug.Log("probeComponent.texture.height = " + probeComponent.texture.height + " blendedCubemap.height = " + blendedCubemap.height);
                    //Debug.Log("probeComponent.resolution = " + probeComponent.resolution);
                    // After the previous RenderProbe is finished, we copy the probe's texture to the cubemap and set it as a custom reflection in RenderSettings.
                    if (probeComponent.texture.width == blendedCubemap.width && probeComponent.texture.height == blendedCubemap.height)
                    {
                        Graphics.CopyTexture(probeComponent.texture, blendedCubemap as Texture);
                        //Debug.Log("Copying");
                    }

                    RenderSettings.customReflection = blendedCubemap;
                }

                renderId = probeComponent.RenderProbe();
            }
        }
    }
    void UpdateLighting()
    {
        DynamicGI.UpdateEnvironment();
    }
    public void UpdateShaderParameters()
    {
        blendedMaterial.SetTexture("_Skybox1", skybox1);
        blendedMaterial.SetTexture("_Skybox2", skybox2);
        blendedMaterial.SetFloat("_Blend", blend);
    }
}
[System.Serializable]
public class ShitToRender
{
    public List<GameObject> shits;
}
