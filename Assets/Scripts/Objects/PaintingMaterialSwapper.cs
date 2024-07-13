using UnityEngine;
using System.Collections;

public class PaintingMaterialSwapper : MonoBehaviour
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;

    private MeshRenderer paintingRenderer;

    private void Awake()
    {
        // Initialize the paintingRenderer with the MeshRenderer component attached to the same GameObject
        paintingRenderer = GetComponent<MeshRenderer>();

        if (paintingRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on this GameObject.");
        }
    }

    private void Start()
    {
        // Initialize the painting with the inactive material
        if (paintingRenderer != null && inactiveMaterial != null)
        {
            paintingRenderer.material = inactiveMaterial;
        }
    }

    // Public function to swap the material immediately
    public void SwapMaterialImmediately()
    {
        if (paintingRenderer != null && activeMaterial != null)
        {
            paintingRenderer.material = activeMaterial;
        }
    }

    // Public function to swap the material after a specified amount of time
    public void SwapMaterialWithDelay(float delay)
    {
        StartCoroutine(SwapMaterialAfterDelay(delay));
    }

    // Coroutine to handle the delayed material swap
    private IEnumerator SwapMaterialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (paintingRenderer != null && activeMaterial != null)
        {
            paintingRenderer.material = activeMaterial;
        }
    }
}
