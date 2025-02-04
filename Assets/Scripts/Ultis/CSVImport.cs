using UnityEngine;

public class CSVImport : MonoBehaviour
{
    public TextAsset mapCSV;
    public int[,] maze;
    public void Init()
    {
        string[] rows = mapCSV.text.Split('\n');

        maze = new int[rows.Length, rows[0].Split(',').Length];
        for (int i = 0; i < rows.Length; i++)
        {
            string[] cols = rows[i].Split(',');
            for (int j = 0; j < cols.Length; j++)
            {
                if (int.TryParse(cols[j].Trim(), out int number))
                {
                    maze[i,j] = number;
                    // Debug.Log( $"Value {number} at {i} {j}");
                }
                else
                {
                    Debug.LogWarning($"Không thể chuyển đổi giá trị: {cols[j]} tại vị trí [{i},{j}]");
                }
            }
        }
    }
}
