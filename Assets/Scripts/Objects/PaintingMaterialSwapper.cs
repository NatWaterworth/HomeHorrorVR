using UnityEngine;
using System.Collections;

public class PaintingMaterialSwapper : MonoBehaviour
{
    [SerializeField] private Material _inactiveMaterial;
    [SerializeField] private Material _activeMaterial;

    [SerializeField] private bool _doDelaySwap;
    [SerializeField] private float _swapDelay;

    private MeshRenderer _paintingRenderer;

    private void Awake()
    {
        // Initialize the paintingRenderer with the MeshRenderer component attached to the same GameObject
        _paintingRenderer = GetComponent<MeshRenderer>();

        if (_paintingRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on this GameObject.");
        }
    }

    private void Start()
    {
        // Initialize the painting with the inactive material
        if (_paintingRenderer != null && _inactiveMaterial != null)
        {
            _paintingRenderer.material = _inactiveMaterial;
        }

        if (_doDelaySwap)
        {
            SwapMaterialAfterDelay(_swapDelay);
        }
    }

    private void OnEnable()
    {
        LightmapManager.OnLightingToggle += SwapMaterialImmediately;
    }

    private void OnDisable()
    {
        LightmapManager.OnLightingToggle -= SwapMaterialImmediately;
    }

    // Public function to swap the material immediately
    public void SwapMaterialImmediately(bool inactive)
    {
        if (_paintingRenderer != null && _activeMaterial != null)
        {
            _paintingRenderer.material = inactive ? _inactiveMaterial : _activeMaterial;
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
        if (_paintingRenderer != null && _activeMaterial != null)
        {
            _paintingRenderer.material = _activeMaterial;
        }
    }
}
