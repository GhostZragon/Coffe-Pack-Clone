using UnityEngine;

public class SinusoidalMovementBase : MonoBehaviour
{
    private enum MovingAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] protected float timePeriod = 2f;
    [SerializeField] protected float height = 30f;
    [SerializeField] private MovingAxis moving = MovingAxis.Y;
    [SerializeField] protected Vector3 startPosition;

    protected void UpdateSinPosition()
    {
        Vector3 nextPos = startPosition;
        switch (moving)
        {
            case MovingAxis.X:
                nextPos.x +=
                    height * Mathf.Sin(((Mathf.PI * 2) / timePeriod) *
                                       Time.time); // Dùng Time.time thay vì timeSinceStart
                break;
            case MovingAxis.Y:
                nextPos.y +=
                    height * Mathf.Sin(((Mathf.PI * 2) / timePeriod) * Time.time); // Dùng Time.time thay vì timeSinceStart
                break;
        }

        transform.localPosition = nextPos;
    }
}