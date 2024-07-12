using UnityEngine;

public class LightBulb : MonoBehaviour
{
    
    private float emissionIntensityOn = 1.0f;
    private float emissionIntensityOff = 0.0f;
    private Material bulbMaterial;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            Debug.LogWarning($"{this} has no mesh renderer to get material from.");
            return;
        }

        bulbMaterial = meshRenderer.material;
    }

    private void OnEnable()
    {
        LightmapManager.OnLightingToggle += UpdateLightBulb;
    }

    private void OnDisable()
    {
        LightmapManager.OnLightingToggle -= UpdateLightBulb;
    }

    private void UpdateLightBulb(bool isOn)
    {
        if (bulbMaterial == null) return;
        float intensity = isOn ? emissionIntensityOn : emissionIntensityOff;
        bulbMaterial.SetColor("_EmissionColor", bulbMaterial.color * intensity);
        DynamicGI.SetEmissive(GetComponent<Renderer>(), bulbMaterial.color * intensity);
    }
}
