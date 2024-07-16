using UnityEngine;

public class LightSwitch : InteractableObject
{
    public Light targetLight;
    public Switch switchComponent;

    // Function to toggle the light on and off
    public void ToggleLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }

        if (switchComponent != null)
        {
            switchComponent.ToggleZScale();
        }
    }

    public override void Select()
    {
        base.Select();
        ToggleLight();
    }
}
