using UnityEngine;

public class TeddyController : MonoBehaviour
{
    public Animator animator;
    public Collider mainCollider;
    public NavigationController navigationController;

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    private void OnEnable()
    {
        LightmapManager.OnLightingToggle += HandleLightingToggle;
    }

    private void OnDisable()
    {
        LightmapManager.OnLightingToggle -= HandleLightingToggle;
    }

    private void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Remove the main collider from the ragdollColliders array
        ragdollColliders = System.Array.FindAll(ragdollColliders, col => col != mainCollider);

        navigationController.Setup(animator); // Pass the Animator reference to NavigationController
        EnableNavMesh();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void HandleLightingToggle(bool lightingOn)
    {
        if (!lightingOn)
        {
            EnableRagdoll();
        }
        else
        {
            EnableNavMesh();
        }
    }

    private void EnableNavMesh()
    {
        SetRagdollState(false);
        animator.enabled = true;
        mainCollider.enabled = true;
        navigationController.EnableNavigation();
    }

    private void EnableRagdoll()
    {
        SetRagdollState(true);
        animator.enabled = false;
        mainCollider.enabled = false;
        navigationController.DisableNavigation();
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !state;
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = state;
        }
    }

    private void UpdateAnimator()
    {
        if (animator.enabled && navigationController.navMeshAgent.enabled)
        {
            animator.SetFloat("MoveSpeed", navigationController.navMeshAgent.velocity.magnitude);
            animator.SetBool("Jump", navigationController.IsJumping());
        }
    }
}
