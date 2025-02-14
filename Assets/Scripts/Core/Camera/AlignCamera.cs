using Sirenix.OdinInspector;
using UnityEngine;

public class AlignCamera : MonoBehaviour
{
    [SerializeField] private Bounds bounds;
    [SerializeField] private Transform map;
    [SerializeField] private float angle = 30;
    [SerializeField] private float offsetLength = 0;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Camera mainCam;
    [Button]
    public void UpdateBound()
    {
        bounds = new();
        foreach (Transform item in map)
        {
            Debug.Log("Add: " + item.transform.name);
            bounds.Encapsulate(item.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (map == null) return;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(bounds.center, CalculateBoxRadius());
        DrawLine();
    }

    float CalculateBoxRadius()
    {
        Vector3 size = bounds.size;

        // Bán kính ngoại tiếp
        float circumradius = 0.5f * Mathf.Sqrt(
            size.x * size.x +
            size.y * size.y +
            size.z * size.z
        );

        // Bán kính nội tiếp
        float inradius = Mathf.Max(size.x, size.z);
        return inradius;
    }

    private void DrawLine()
    {
        Vector3 startPoint = bounds.center;
        float angleInRadians = angle * Mathf.Deg2Rad;
        
        // Tạo một quaternion để xoay vector
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);
        
        // Tính toán vector hướng ban đầu dựa trên góc
        Vector3 baseDirection = new Vector3(
            Mathf.Cos(angleInRadians),
            Mathf.Sin(angleInRadians),
            0
        );
        
        // Áp dụng xoay cho vector hướng
        Vector3 rotatedDirection = rotation * baseDirection;
        
        // Tính điểm cuối
        Vector3 endPoint = startPoint + rotatedDirection * (CalculateBoxRadius() + offsetLength);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPoint, endPoint);

        if (mainCam == null) return;
        mainCam.transform.position = endPoint;
    }
    
    [Button]
    public void UpdateOffsetLength()
    {
        offsetLength = Mathf.Max(bounds.extents.x, bounds.extents.z) * 10;
        offsetLength += offsetLength * 0.15f;
    }
}