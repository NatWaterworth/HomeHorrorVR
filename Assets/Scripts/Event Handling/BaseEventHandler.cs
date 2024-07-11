using UnityEngine;
using System.Collections.Generic;

public abstract class BaseEventHandler : MonoBehaviour
{
    public bool IsActiveEvent;
    public int EventActionPoints;
    public int sectorNumber; // The sector number this event belongs to
    public List<BaseEventHandler> PreviousEvents; // List of previous events that lead to this event

    // Abstract methods for interaction and applying effects
    public abstract void Interact(GameObject playerItem = null);
    public abstract void ApplyEffect();
}
