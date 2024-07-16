using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickupObject : InteractableObject, IPickUpable
{
    public virtual void Activate()
    {
        _soundController?.PlayActivateSound();
        Debug.Log("The object is activated.");
    }

    public virtual void Deactivate()
    {
        _soundController?.PlayDeactivateSound();
        Debug.Log("The object is deactivated.");
    }

    protected override void Awake()
    {
        base.Awake();

        _grabInteractable.activated.AddListener(OnActivate);
        _grabInteractable.deactivated.AddListener(OnDeactivate);
    }

    protected override void OnDestroy()
    {

        _grabInteractable.activated.RemoveListener(OnActivate);
        _grabInteractable.deactivated.RemoveListener(OnDeactivate);
    }

    private void OnActivate(ActivateEventArgs args)
    {
        Activate();
    }

    private void OnDeactivate(DeactivateEventArgs args)
    {

        Deactivate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        BasicObject basicObject = collision.gameObject.GetComponent<BasicObject>();

        if (basicObject != null)
        {
            Vector3 collisionPoint = collision.contacts[0].point;

            float relativeVelocity = collision.relativeVelocity.magnitude;

            SoundManager.Instance.PlayImpactSound(basicObject.materialType, collisionPoint, relativeVelocity);
        }
    }
}
