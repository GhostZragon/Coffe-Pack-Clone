using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AlignItemBase : MonoBehaviour
{
    [SerializeField] private Transform[] items;
    [SerializeField] bool centerPartialRows = true; // Tự động căn giữa cho các hàng không đủ item
    [SerializeField] private bool isDrawGizmos = false;
    [SerializeField] private float size = .5f;
    [Button]
    public void Alin(int count)
    {
        if (items == null || items.Length == 0) return;

        // Tính toán kích thước mặt phẳng dựa trên scale
        Vector3 planeSize = GetPlaneSize(); // Plane mặc định 10x10 unit

        // int itemCount = items.Length;
        int cols = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt((float)count / cols);

        // Tính kích thước mỗi ô lưới
        Vector2 cellSize = new Vector2(
            planeSize.x / cols,
            planeSize.z / rows
        );

        for (int i = 0; i < count; i++)
        {
            int row = i / cols;
            int col = i % cols;

            // Tính offset cho các hàng không đầy đủ
            float xOffset = 0;
            if (centerPartialRows && row == rows - 1)
            {
                int itemsInRow = count - (row * cols);
                if (itemsInRow < cols)
                {
                    xOffset = (planeSize.x - itemsInRow * cellSize.x) / 2;
                }
            }

            // Tính vị trí
            Vector3 newPos = new Vector3(
                -planeSize.x / 2 + xOffset + cellSize.x * (col + 0.5f),
                0f,
                planeSize.z / 2 - cellSize.y * (row + 0.5f)
            );

            // Áp dụng vị trí
            items[i].localPosition = newPos;
        }
    }

    protected abstract Vector3 GetPlaneSize();


    private void OnDrawGizmos()
    {
        if (isDrawGizmos == false) return;

        for (int i = 0; i < items.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(items[i].transform.position,Vector3.one * size);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,GetPlaneSize());
    }
}