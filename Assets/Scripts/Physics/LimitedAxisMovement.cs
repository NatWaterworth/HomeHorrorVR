using UnityEngine;

public class LimitedAxisMovement : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    public Axis movementAxis = Axis.X;

    public float startPosition = 0f;
    public float endPosition = 10f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial position and rotation to maintain other axis values and lock rotation
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        LimitMovement();
    }

    void LimitMovement()
    {
        Vector3 localPos = transform.localPosition;

        switch (movementAxis)
        {
            case Axis.X:
                localPos.x = Mathf.Clamp(localPos.x, startPosition, endPosition);
                localPos.y = initialPosition.y;
                localPos.z = initialPosition.z;
                break;
            case Axis.Y:
                localPos.y = Mathf.Clamp(localPos.y, startPosition, endPosition);
                localPos.x = initialPosition.x;
                localPos.z = initialPosition.z;
                break;
            case Axis.Z:
                localPos.z = Mathf.Clamp(localPos.z, startPosition, endPosition);
                localPos.x = initialPosition.x;
                localPos.y = initialPosition.y;
                break;
        }

        transform.localPosition = localPos;

        // Lock rotation
        transform.localRotation = initialRotation;
    }
}
