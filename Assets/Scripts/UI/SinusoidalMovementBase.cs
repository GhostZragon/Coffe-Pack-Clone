using Sirenix.OdinInspector;
using UnityEngine;

public class SinusoidalMovementBase : MonoBehaviour
{
    private enum MovingAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
        XY = 3,
        XZ = 4,
        YZ = 5,
    }

    [SerializeField] protected float timePeriod = 2f;
    [SerializeField] protected float height = 30f;
    [SerializeField, EnumToggleButtons] private MovingAxis moving = MovingAxis.Y;
    [SerializeField] protected Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    protected void UpdateSinPosition()
    {
        Vector3 nextPos = startPosition;
        float sinValue = Mathf.Sin((Mathf.PI * 2 / timePeriod) * Time.time) * height;

        switch (moving)
        {
            case MovingAxis.X:
                nextPos.x += sinValue;
                break;
            case MovingAxis.Y:
                nextPos.y += sinValue;
                break;
            case MovingAxis.Z:
                nextPos.z += sinValue;
                break;
            case MovingAxis.XY:
                nextPos.x += sinValue;
                nextPos.y += sinValue;
                break;
            case MovingAxis.XZ:
                nextPos.x += sinValue;
                nextPos.z += sinValue;
                break;
            case MovingAxis.YZ:
                nextPos.y += sinValue;
                nextPos.z += sinValue;
                break;
        }

        transform.localPosition = nextPos;
    }

    private void Update()
    {
        UpdateSinPosition();
    }
}