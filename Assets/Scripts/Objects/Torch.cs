using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light flashlightLight; // Assign the Light component of the flashlight
    [SerializeField] private Renderer flashlightRenderer; // Assign the Renderer component of the flashlight
    [SerializeField] private Material flashlightMaterial; // Assign the material that supports emission

    void Start()
    {
        if (flashlightLight == null || flashlightRenderer == null || flashlightMaterial == null)
        {
            Debug.LogError("Please assign all required components.");
            return;
        }

        // Initialize the flashlight to be off
        SetFlashlightState(false);
    }

    public void TurnFlashlightOn()
    {
        SetFlashlightState(true);
    }

    public void TurnFlashlightOff()
    {
        SetFlashlightState(false);
    }

    private void SetFlashlightState(bool state)
    {
        flashlightLight.enabled = state;

        if (state)
        {
            flashlightMaterial.EnableKeyword("_EMISSION");
            flashlightRenderer.material.SetColor("_EmissionColor", Color.white); // Adjust color as needed
        }
        else
        {
            flashlightMaterial.DisableKeyword("_EMISSION");
            flashlightRenderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
