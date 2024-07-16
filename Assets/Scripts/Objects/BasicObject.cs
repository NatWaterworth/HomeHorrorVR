using UnityEngine;

public enum MaterialType
{
    Wood,
    Metal,
    Soft,
    Glass,
    Concrete
}

public class BasicObject : MonoBehaviour
{
    public MaterialType materialType;
}