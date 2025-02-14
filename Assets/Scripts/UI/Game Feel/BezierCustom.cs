using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BezierCustom
{
    public Vector3 startPosition;
    public Vector3 point2;
    public Vector3 point3;
    public Vector3 endPosition;

    public Vector3 moveObjectPosition;

    private void Setup(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;

        float distance = Vector3.Distance(startPosition, endPosition);
        float offsetY = distance * 0.5f;

        bool isLeft = Random.Range(0, 2) == 0; // Random hướng cong
        var randomValue = Random.Range(startPosition.y/2, this.endPosition.y/2);
        float offsetX = isLeft ?randomValue : -randomValue; // Dịch sang trái/phải
        if (offsetX < 1 && offsetX > -1)
        {
            offsetX = Mathf.Clamp(offsetX * 10, -1, 1);
        }
        point2 = Vector2.Lerp(startPosition, endPosition, 0.3f) + new Vector2(offsetX, 0);
        point3 = Vector2.Lerp(startPosition, endPosition, 0.6f) + new Vector2(-offsetX, 0);
    }

    public static BezierCustom Create(Vector3 startPosition, Vector3 endPosition)
    {
        BezierCustom bezierCustom = new();
        
        bezierCustom.Setup(startPosition,endPosition);

        return bezierCustom;
    }
    
    
    public void Play1(float t)
    {
        // Công thức Bézier bậc 1: B(t) = (1-t) * P₀ + t * P₁
        moveObjectPosition = (1 - t) * startPosition + t * endPosition;
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