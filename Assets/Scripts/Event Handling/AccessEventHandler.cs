using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AccessEventHandler : BaseEventHandler
{
    public List<Lock> lockOptions;
    public Lock activeLock;

    protected virtual void Awake()
    {
        if (lockOptions == null || lockOptions.Count == 0)
        {
            Debug.LogError($"{this} has no Lock Options");
            return;
        }

        SelectRandomLock();
    }

    private void SelectRandomLock()
    {
        int randomIndex = Random.Range(0, lockOptions.Count);
        activeLock = lockOptions[randomIndex];

        for (int i = 0; i < lockOptions.Count; i++)
        {
            if (i == randomIndex)
            {
                lockOptions[i].gameObject.SetActive(true);
            }
            else
            {
                lockOptions[i].gameObject.SetActive(false);
            }
        }
    }

    public override void Interact(GameObject playerItem = null)
    {
        if (activeLock && activeLock.TryUnlock())
        {
            ApplyEffect();
        }
    }
}
