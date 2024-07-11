using UnityEngine;

public class CodePuzzleLock : Lock
{
    public string correctCode;
    public GameObject clueObject; // The object that gives the clue for the code
    private string playerInputCode;

    public void SetPlayerInputCode(string code)
    {
        playerInputCode = code;
    }

    public override bool TryUnlock()
    {
        return playerInputCode == correctCode;
    }
}
