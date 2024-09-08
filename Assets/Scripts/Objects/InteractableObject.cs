using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ObjectSoundController))]
public abstract class InteractableObject : BasicObject, IInteractable
{
    protected ObjectSoundController _soundController;
    protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable _grabInteractable;
    protected Rigidbody _rigidbody;

    private float _minMoveVelocity = 0.01f;
    private float _maxMoveThresholdVelocity = 10f; 
    private bool _isMoving;

    protected void FixedUpdate()
    {
        OnMove();
    }

    public void OnMove()
    {
        if (_rigidbody == null) return;

        float velocityMagnitude = _rigidbody.linearVelocity.magnitude;

        HandleMovementState(velocityMagnitude);
        UpdateSoundSpeed(velocityMagnitude);
    }

    private void HandleMovementState(float velocityMagnitude)
    {
        bool isBelowMinVelocity = velocityMagnitude < _minMoveVelocity;
        bool isAboveMinVelocity = velocityMagnitude > _minMoveVelocity;

        if (isBelowMinVelocity && _isMoving)
        {
            StopMovement();
        }
        else if (isAboveMinVelocity && !_isMoving)
        {
            StartMovement();
        }
    }

    private void StopMovement()
    {
        _soundController?.StopSound();
        _isMoving = false;
    }

    private void StartMovement()
    {
        _soundController?.PlayMoveSound();
        _isMoving = true;
    }

    private void UpdateSoundSpeed(float velocityMagnitude)
    {
        if (velocityMagnitude > _minMoveVelocity)
        {
            float normalizedSpeed = Mathf.InverseLerp(0, _maxMoveThresholdVelocity, velocityMagnitude);
            _soundController?.UpdateMoveSoundSpeed(normalizedSpeed);
        }
    }

    public virtual void Select()
    {
        _soundController?.PlaySelectSound();
        // Specific interact functionality for a pickup object
        Debug.Log("The object is being activated.");
    }

    public virtual void Deselect()
    {
        _soundController?.PlayDeselectSound();
        // Specific interact functionality for a pickup object
        Debug.Log("The object is being deactivated.");
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

    protected virtual void Awake()
    {
        // Ensure the ObjectSoundController component is attached to the same GameObject
        _soundController = GetComponent<ObjectSoundController>();

        if (_soundController == null)
        {
            Debug.LogError("ObjectSoundController component not found on this GameObject.");
        }

        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        if (_grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on this GameObject.");
        }

        _rigidbody = GetComponent<Rigidbody>();

        _grabInteractable.firstHoverEntered.AddListener(OnFirstHoverEntered);
        _grabInteractable.lastHoverExited.AddListener(OnLastHoverExited);
        _grabInteractable.selectEntered.AddListener(OnSelectEntered);
        _grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    protected virtual void OnDestroy()
    {
        _grabInteractable.firstHoverEntered.RemoveListener(OnFirstHoverEntered);
        _grabInteractable.lastHoverExited.RemoveListener(OnLastHoverExited); 
        _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        _grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnFirstHoverEntered(HoverEnterEventArgs args)
    {
        Highlight();
    }

    private void OnLastHoverExited(HoverExitEventArgs args)
    {
        Unhighlight();
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Select();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        Deselect();
    }
}
