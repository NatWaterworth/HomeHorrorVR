using UnityEngine;

public class LightSwitch : InteractableObject
{
    [SerializeField] Switch _switchComponent;

    // Function to toggle the light on and off
    public void ToggleLight()
    {
        LightmapManager.Instance.ToggleLightmaps();

        if (_switchComponent != null)
        {
            _switchComponent.ToggleZScale();
        }
    }

    public override void Select()
    {
        base.Select();
        ToggleLight();
    }
}
