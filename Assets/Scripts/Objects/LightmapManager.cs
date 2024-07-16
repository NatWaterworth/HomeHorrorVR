using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] _darkLightMapDir;
    [SerializeField] private Texture2D[] _darkLightMapColour;
    [SerializeField] private Texture2D[] _brightLightMapDir;
    [SerializeField] private Texture2D[] _brightLightMapColour;
    [SerializeField] private LightProbesData _darkLightProbesData;
    [SerializeField] private LightProbesData _brightLightProbesData;

    private ReflectionProbeSwitcher[] reflectionProbeSwitchers;
    private LightmapData[] darkLightMap;
    private LightmapData[] brightLightMap;

    private bool lightingOn = true;

    public static event Action<bool> OnLightingToggle;

    public static LightmapManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the instance alive across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    void Start()
    {
        SetupLightmaps();
        SetupReflectionProbeControllers();
        OnLightingToggle?.Invoke(lightingOn);
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
        SetLightProbes(lightingOn);
        OnLightingToggle?.Invoke(lightingOn);
    }

    private void SetReflectionProbe(ReflectionProbe[] probes, bool isOn)
    {
        foreach (var probe in probes)
        {
            probe.gameObject.SetActive(isOn);
        }
    }

    private void SetLightProbes(bool isOn)
    {
        LightmapSettings.lightProbes.bakedProbes = isOn ?
            _brightLightProbesData.bakedProbes : _darkLightProbesData.bakedProbes;
    }

    private void SetupLightmaps()
    {
        darkLightMap = SetupLightMap(_darkLightMapDir, _darkLightMapColour);
        brightLightMap = SetupLightMap(_brightLightMapDir, _brightLightMapColour);
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
