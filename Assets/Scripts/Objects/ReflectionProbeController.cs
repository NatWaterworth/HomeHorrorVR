using System.Diagnostics;
using UnityEngine;

public class ReflectionProbeSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject darkReflectionProbesParent;
    [SerializeField] private GameObject lightReflectionProbesParent;

    private bool isLight = true;

    void Start()
    {
        // Ensure the initial state is set correctly
        UpdateReflectionProbes();
    }

    public void ToggleReflectionProbes()
    {
        isLight = !isLight;
        UpdateReflectionProbes();
    }

    public void SetReflectionProbes(bool light)
    {
        isLight = light;
        UpdateReflectionProbes();
    }

    private void UpdateReflectionProbes()
    {
        if (darkReflectionProbesParent == null || lightReflectionProbesParent == null)
        {
            UnityEngine.Debug.LogError("Please assign the dark and light reflection probes parent GameObjects in the inspector.");
            return;
        }

        darkReflectionProbesParent.SetActive(!isLight);
        lightReflectionProbesParent.SetActive(isLight);
    }
}
