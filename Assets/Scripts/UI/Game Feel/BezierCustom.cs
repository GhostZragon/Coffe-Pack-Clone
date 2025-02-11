using System;
using UnityEngine;

[Serializable]
public class BezierCustom : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 point2;
    public Vector3 point3;
    public Vector3 endPosition;

    public Vector3 moveObjectPosition;

    public void Play1(float t)
    {
        // Công thức Bézier bậc 1: B(t) = (1-t) * P₀ + t * P₁
        moveObjectPosition = (1 - t) * startPosition + t * point2;
    }

    public void Play2(float t)
    {
        // Công thức Bézier bậc 2: B(t) = (1-t)² * P₀ + 2(1-t)t * P₁ + t² * P₂
        moveObjectPosition = Mathf.Pow(1 - t, 2) * startPosition +
                             2 * (1 - t) * t * point2 +
                             Mathf.Pow(t, 2) * point3;
    }

    public void Play3(float t)
    {
        // Công thức Bézier bậc 3: 
        // B(t) = (1-t)³ * P₀ + 3(1-t)²t * P₁ + 3(1-t)t² * P₂ + t³ * P₃
        moveObjectPosition = Mathf.Pow(1 - t, 3) * startPosition +
                             3 * Mathf.Pow(1 - t, 2) * t * point2 +
                             3 * (1 - t) * Mathf.Pow(t, 2) * point3 +
                             Mathf.Pow(t, 3) * endPosition;
    }
}