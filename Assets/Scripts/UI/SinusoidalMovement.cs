using Sirenix.OdinInspector;
using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    public float timePeriod = 2f;
    public float height = 30f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition;  // Dùng localPosition thay vì position
        timePeriod = Random.Range(7f, 10f);
    }
    [Button]
    public void Test()
    {
        timePeriod = Random.Range(7f, 10f);
    }
    private void Update()
    {
        UpdateSinPosition();
    }

    private void UpdateSinPosition()
    {
        Vector3 nextPos = startPosition;
        nextPos.y += height * Mathf.Sin(((Mathf.PI * 2) / timePeriod) * Time.time); // Dùng Time.time thay vì timeSinceStart

        transform.localPosition = nextPos;
    }
}