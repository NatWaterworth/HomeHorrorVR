using UnityEngine;

public class SectorEventHandler : BaseEventHandler
{
    public int unlockedSector;
    public GameObject lockArea;
    public GameObject nextSectorKeyArea;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Initially prevent movement until unlocked
        }
    }

    public override void Interact(GameObject playerItem = null)
    {
        // Implement interaction logic, e.g., check if the player has the correct key
    }

    public override void ApplyEffect()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Allow movement when unlocked
        }
        // Implement additional effect logic, e.g., place the key in the nextSectorKeyArea
    }
}
