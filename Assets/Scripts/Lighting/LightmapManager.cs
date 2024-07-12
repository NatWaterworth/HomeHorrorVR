using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightmapManager : MonoBehaviour
{
    private LightmapData[] originalLightmaps;
    private LightProbes originalLightProbes;
    private bool lightmapsEnabled = true;

    public Volume postProcessVolume; // Assign your Post-Processing Volume in the inspector

    private ColorAdjustments colorGrading;

    void Start()
    {
        // Save the original lightmaps and light probes
        originalLightmaps = LightmapSettings.lightmaps;
        originalLightProbes = LightmapSettings.lightProbes;

        // Get the Color Grading component from the Post-Processing Volume
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out colorGrading);
        }
    }

    public void SetLightmapIntensity(float intensity)
    {
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = intensity;
        }
    }

    public void ToggleLightmaps()
    {
        lightmapsEnabled = !lightmapsEnabled;
        LightmapSettings.lightmaps = lightmapsEnabled ? originalLightmaps : null;
        LightmapSettings.lightProbes = lightmapsEnabled ? originalLightProbes : null;
    }

    public void TurnLightmapsOn()
    {
        lightmapsEnabled = true;
        LightmapSettings.lightmaps = originalLightmaps;
        LightmapSettings.lightProbes = originalLightProbes;
    }

    public void TurnLightmapsOff()
    {
        lightmapsEnabled = false;
        LightmapSettings.lightmaps = null;
        LightmapSettings.lightProbes = null;
    }

    public void FlickerLightmaps(float duration)
    {
        StartCoroutine(FlickerCoroutine(duration));
    }

    private IEnumerator FlickerCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            // Set a random post-exposure value between 0 and 1
            float randomIntensity = Random.Range(0f, 1f);
            SetLightmapIntensity(randomIntensity);
            // Wait for a random interval between 0.05 and 0.2 seconds
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
        TurnLightmapsOff();
    }
}
