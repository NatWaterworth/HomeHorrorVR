using UnityEngine;

public class Flashlight : PickupObject
{
    [SerializeField] private Light _flashlightLight; // Assign the Light component of the flashlight
    [SerializeField] private Renderer _flashlightRenderer; // Assign the Renderer component of the flashlight

    private Material _flashlightMaterial;
    private bool _isOn;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

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

    public override void Activate()
    {
        base.Activate();
        TurnFlashlightOn();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        TurnFlashlightOff();
    }

    public override void Deselect()
    {
        base.Deselect();
        TurnFlashlightOff();
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
