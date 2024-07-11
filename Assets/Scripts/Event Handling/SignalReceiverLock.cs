using UnityEngine;

public class SignalReceiverLock : Lock
{
    private bool isUnlocked = false;

    public void ReceiveSignal()
    {
        isUnlocked = true;
    }

    public override bool TryUnlock()
    {
        return isUnlocked;
    }
}
