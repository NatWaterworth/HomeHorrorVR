using UnityEngine;

public class Flashlight : PickupObject
{
    [SerializeField] private Light _flashlightLight; // Assign the Light component of the flashlight
    [SerializeField] private Renderer _flashlightRenderer; // Assign the Renderer component of the flashlight

    private Material _flashlightMaterial;
    private bool _isOn;


    void Start()
    {
        if (_flashlightLight == null || _flashlightRenderer == null)
        {
            Debug.LogError("Please assign all required components.");
            return;
        }
        _flashlightMaterial = _flashlightRenderer.material;


        SetFlashlightState(false);
    }

    public override void Select()
    {
        base.Select();
        ToggleFlashlight();
    }

    public void ToggleFlashlight()
    {
        _isOn = !_isOn;
        SetFlashlightState(_isOn);
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
        _flashlightLight.enabled = state;
        _isOn = state;
        if (state)
        {
            _flashlightMaterial.EnableKeyword("_EMISSION");
            _flashlightRenderer.material.SetColor("_EmissionColor", Color.white); // Adjust color as needed
        }
        else
        {
            _flashlightMaterial.DisableKeyword("_EMISSION");
            _flashlightRenderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
