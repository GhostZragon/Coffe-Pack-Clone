using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SinusoidalAutoMovement : SinusoidalMovementBase
{
    [SerializeField] private bool IsRandomPeriod = false;
    private void Awake()
    {
        startPosition = transform.localPosition;  // Dùng localPosition thay vì position
        if (IsRandomPeriod)
        {
            timePeriod = Random.Range(7f, 10f);
        }
    }

    private void Update()
    {
        UpdateSinPosition();
    }

    
}