using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool isOn = false;

    // Function to toggle the Z scale (flip the switch)
    public void ToggleZScale()
    {
        Vector3 scale = transform.localScale;
        scale.z = isOn ? Mathf.Abs(scale.z) : -Mathf.Abs(scale.z);
        transform.localScale = scale;
        isOn = !isOn;
    }
}
