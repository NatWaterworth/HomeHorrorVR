using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EventTreeManager : MonoBehaviour
{
    public List<BaseEventHandler> eventHandlers;
    public int totalEventActionPoints;
    public KeyLockPairList containerKeyLockPairsList;
    public KeyLockPairList puzzleKeyLockPairsList;

    private Dictionary<string, bool> playerItems = new Dictionary<string, bool>();
    private List<KeyLockPair> usedKeyLockPairs = new List<KeyLockPair>();
    private int currentSector = 0;

    void Start()
    {
        GenerateEventTree();
    }

    void GenerateEventTree()
    {
        int currentActionPoints = 0;
        List<BaseEventHandler> possibleEvents = new List<BaseEventHandler>(eventHandlers);
        BaseEventHandler rootEvent = possibleEvents.OfType<EndGameEventHandler>().FirstOrDefault();

        if (rootEvent == null)
        {
            Debug.LogError("No EndGameEventHandler found!");
            return;
        }

        rootEvent.IsActiveEvent = true;
        currentSector = rootEvent.sectorNumber;
        possibleEvents.Remove(rootEvent);

        Stack<BaseEventHandler> eventStack = new Stack<BaseEventHandler>();
        eventStack.Push(rootEvent);

        while (currentActionPoints < totalEventActionPoints && eventStack.Count > 0)
        {
            BaseEventHandler currentEvent = eventStack.Pop();
            List<BaseEventHandler> eligibleEvents = GetEligibleEvents(possibleEvents, currentSector);

            if (eligibleEvents.Count == 0 && currentSector > 0)
            {
                // If no eligible events in the current sector, decrement the sector
                currentSector--;
                eligibleEvents = GetEligibleEvents(possibleEvents, currentSector);
            }

            if (eligibleEvents.Count > 0)
            {
                BaseEventHandler selectedEvent = eligibleEvents[Random.Range(0, eligibleEvents.Count)];
                selectedEvent.IsActiveEvent = true;
                currentActionPoints += selectedEvent.EventActionPoints;
                possibleEvents.Remove(selectedEvent);
                AssignKeyAndLock(selectedEvent, currentEvent);
                eventStack.Push(selectedEvent);

                if (selectedEvent is SectorEventHandler)
                {
                    SectorEventHandler sectorEvent = selectedEvent as SectorEventHandler;
                    if (sectorEvent.unlockedSector == currentSector)
                    {
                        currentSector--;
                    }
                }
            }
        }

        // Set the remaining event handlers to their default inactive state
        foreach (BaseEventHandler handler in possibleEvents)
        {
            handler.IsActiveEvent = false;
        }
    }

    List<BaseEventHandler> GetEligibleEvents(List<BaseEventHandler> events, int sector)
    {
        return events.Where(e => e.sectorNumber <= sector && !(e is EndGameEventHandler)).ToList();
    }

    void AssignKeyAndLock(BaseEventHandler previousEvent, BaseEventHandler currentEvent)
    {
        if (currentEvent is ContainerEventHandler containerEvent)
        {

        }
        else if (currentEvent is RemoteEventHandler remoteTriggerEvent)
        {
            // No need to instantiate a key or lock, just set up the remote trigger
        }
    }

    KeyLockPair GetUnusedKeyLockPair(KeyLockPairList keyLockPairList)
    {
        foreach (var pair in keyLockPairList.keyLockPairs)
        {
            if (!usedKeyLockPairs.Contains(pair))
            {
                usedKeyLockPairs.Add(pair);
                return pair;
            }
        }
        return null; // No more unused pairs available
    }

  
}
