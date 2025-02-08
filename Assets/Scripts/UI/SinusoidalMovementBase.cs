using UnityEngine;

public class SinusoidalMovementBase : MonoBehaviour
{
    [SerializeField] protected float timePeriod = 2f;
    [SerializeField]protected float height = 30f;

    [SerializeField] protected Vector3 startPosition;
    protected void UpdateSinPosition()
    {
        Vector3 nextPos = startPosition;
        nextPos.y += height * Mathf.Sin(((Mathf.PI * 2) / timePeriod) * Time.time); // Dùng Time.time thay vì timeSinceStart

        transform.localPosition = nextPos;
    }
}