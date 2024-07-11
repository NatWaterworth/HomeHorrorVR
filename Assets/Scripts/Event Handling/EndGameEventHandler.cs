using UnityEngine;

public class EndGameEventHandler : BaseEventHandler
{
    public override void Interact(GameObject playerObejct)
    {
        // Implement interaction logic to end the game
        Debug.Log("Game Ended!");
    }

    public override void ApplyEffect()
    {
        // Implement effect logic for ending the game
    }
}
