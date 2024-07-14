using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickupObject : InteractableObject, IPickUpable
{
    protected override void Awake()
    {
        base.Awake();

        _grabInteractable.selectEntered.AddListener(OnSelectEntered);
        _grabInteractable.selectExited.AddListener(OnSelectExited);
        _grabInteractable.activated.AddListener(OnActivate);
        _grabInteractable.deactivated.AddListener(OnDeactivate);
    }

    protected override void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        _grabInteractable.selectExited.RemoveListener(OnSelectExited);
        _grabInteractable.activated.RemoveListener(OnActivate);
        _grabInteractable.deactivated.RemoveListener(OnDeactivate);
    }

    public virtual void Grab()
    {
        _soundController?.PlayGrabSound();
        Debug.Log("The object is grabbed.");
        // Add code to handle grabbing the object
    }

    public virtual void Release()
    {
        _soundController?.PlayReleaseSound();
        Debug.Log("The object is released.");
        // Add code to handle releasing the object
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Grab();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        Release();
    }

    private void OnActivate(ActivateEventArgs args)
    {
        _soundController?.PlayActivateSound();
        Debug.Log("The object is activated.");
    }

    private void OnDeactivate(DeactivateEventArgs args)
    {
        _soundController?.PlayDeactivateSound();
        Debug.Log("The object is deactivated.");
    }
}
