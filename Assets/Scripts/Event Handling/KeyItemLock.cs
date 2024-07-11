using UnityEngine;

public class KeyItemLock : Lock
{
    public GameObject keyPrefab;
    private GameObject playerItem;

    public void SetPlayerItem(GameObject item)
    {
        playerItem = item;
    }

    public override bool TryUnlock()
    {
        return playerItem == keyPrefab;
    }
}
