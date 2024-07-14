using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ObjectSoundController))]
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    protected ObjectSoundController _soundController;
    protected XRGrabInteractable _grabInteractable;

    protected virtual void Awake()
    {
        // Ensure the ObjectSoundController component is attached to the same GameObject
        _soundController = GetComponent<ObjectSoundController>();

        if (_soundController == null)
        {
            Debug.LogError("ObjectSoundController component not found on this GameObject.");
        }

        _grabInteractable = GetComponent<XRGrabInteractable>();

        if (_grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on this GameObject.");
        }

        _grabInteractable.firstHoverEntered.AddListener(OnFirstHoverEntered);
        _grabInteractable.lastHoverExited.AddListener(OnLastHoverExited);
        _grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        _grabInteractable.hoverExited.AddListener(OnHoverExited);
    }

    protected virtual void OnDestroy()
    {
        _grabInteractable.firstHoverEntered.RemoveListener(OnFirstHoverEntered);
        _grabInteractable.lastHoverExited.RemoveListener(OnLastHoverExited);
        _grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        _grabInteractable.hoverExited.RemoveListener(OnHoverExited);
    }

    public virtual void Interact()
    {
        _soundController?.PlayInteractSound();
        // Specific interact functionality for a pickup object
        Debug.Log("The object is being interacted with.");
    }

    public virtual void Highlight()
    {
        // Default highlight functionality
        Debug.Log($"{gameObject.name} highlighted.");
    }

    public virtual void Unhighlight()
    {
        // Default unhighlight functionality
        Debug.Log($"{gameObject.name} unhighlighted.");
    }

    private void OnFirstHoverEntered(HoverEnterEventArgs args)
    {
        Highlight();
    }

    private void OnLastHoverExited(HoverExitEventArgs args)
    {
        Unhighlight();
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Additional functionality for hover entered can be added here
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        // Additional functionality for hover exited can be added here
    }
}
