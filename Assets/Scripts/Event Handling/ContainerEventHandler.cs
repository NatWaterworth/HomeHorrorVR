using System.Collections.Generic;
using UnityEngine;

public class ContainerEventHandler : AccessEventHandler
{
    public BoxCollider keyArea;

    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Initially prevent movement until unlocked
        }
    }

    public override void ApplyEffect()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Allow movement when unlocked
        }
        // Implement additional effect logic, e.g., place the key in the keyArea
    }
}
