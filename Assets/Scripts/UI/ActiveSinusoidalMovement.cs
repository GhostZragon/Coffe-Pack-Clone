using UnityEngine;

public class ActiveSinusoidalMovement : SinusoidalMovementBase
{
    [SerializeField] public bool IsActive = false;
    private void Update()
    {
        if (IsActive)
        {
            UpdateSinPosition();
        }
    }
}