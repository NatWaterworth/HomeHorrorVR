using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapManager : MonoBehaviour
{
    [SerializeField] Texture2D[] darkLightMapDir;
    [SerializeField] Texture2D[] darkLightMapColour;
    [SerializeField] Texture2D[] brightLightMapDir;
    [SerializeField] Texture2D[] brightLightMapColour;

    [SerializeField] ReflectionProbeSwitcher[] reflectionProbeSwitchers;

    private LightmapData[] darkLightMap;
    private LightmapData[] brightLightMap;

    private bool lightingOn = true;

    void Start()
    {
        SetupLightmaps();
        SetupReflectionProbeControllers();
    }

    public void ToggleLightmaps()
    {
        lightingOn = !lightingOn;
        UpdateLighting();
    }

    public void TurnLightmapsOn()
    {
        lightingOn = true;
        UpdateLighting();
    }

    public void TurnLightmapsOff()
    {
        lightingOn = false;
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        LightmapSettings.lightmaps = lightingOn ? brightLightMap : darkLightMap;
        TurnAllReflectionProbesToState(lightingOn);
    }

    private void SetReflectionProbe(ReflectionProbe[] probes, bool isOn)
    {
        foreach (var probe in probes)
        {
            probe.gameObject.SetActive(isOn);
        }
    }

    private void SetupLightmaps()
    {
        darkLightMap = SetupLightMap(darkLightMapDir, darkLightMapColour);
        brightLightMap = SetupLightMap(brightLightMapDir, brightLightMapColour);
    }

    private void SetupReflectionProbeControllers()
    {
        reflectionProbeSwitchers = FindObjectsOfType<ReflectionProbeSwitcher>();
    }

    public void TurnAllReflectionProbesToState(bool isOn)
    {
        if (reflectionProbeSwitchers == null || reflectionProbeSwitchers.Length == 0)
        {
            Debug.LogWarning("No ReflectionProbeSwitchers to turn on.");
            return;
        }

        foreach (var switcher in reflectionProbeSwitchers)
        {
            switcher.SetReflectionProbes(isOn);
        }
    }

    private LightmapData[] SetupLightMap(Texture2D[] dirMaps, Texture2D[] colourMaps)
    {
        List<LightmapData> lightMaps = new List<LightmapData>();

        for (int i = 0; i < dirMaps.Length; i++)
        {
            LightmapData lightmapData = new LightmapData();

            lightmapData.lightmapDir = dirMaps[i];
            lightmapData.lightmapColor = colourMaps[i];

            lightMaps.Add(lightmapData);
        }
        return lightMaps.ToArray();
    }
}
