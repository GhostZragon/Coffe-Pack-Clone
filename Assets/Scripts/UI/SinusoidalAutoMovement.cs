using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SinusoidalAutoMovement : SinusoidalMovementBase
{
   
    private void Awake()
    {
        startPosition = transform.localPosition;  // Dùng localPosition thay vì position

    }

    [Button]
    public void Test()
    {
    }
    private void Update()
    {
        UpdateSinPosition();
    }

    
}