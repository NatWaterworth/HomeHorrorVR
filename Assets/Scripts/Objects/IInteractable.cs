using UnityEngine;

public interface IInteractable
{
    void Select();
    void Deselect();
    void Highlight();
    void Unhighlight();
    void OnMove();

}
