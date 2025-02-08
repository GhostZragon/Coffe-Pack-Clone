using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SinusoidalAutoMovement : SinusoidalMovementBase
{
   
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

    
}