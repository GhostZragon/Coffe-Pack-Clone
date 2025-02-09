using UnityEngine;

public class PathNodesVisualize : MonoBehaviour
{
    [SerializeField] private MapLevelUI mapLevelUI;
    [SerializeField] private float sizeNode = 0.1f;
    [SerializeField] private float distancePerNode = 0.1f;   
    private Transform[] points;
    private void Awake()
    {
        points = mapLevelUI.GetAllSpawnPoints();
    }

    private void OnDrawGizmos()
    {

        if (mapLevelUI == null) return;
        if (points == null || points.Length == 0) return;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 firstPosition = points[i].position;
            Vector3 secondPosition = points[i + 1].position;

            // Vẽ đường nối giữa các level nodes
            Gizmos.DrawLine(firstPosition, secondPosition);

            float distance = Vector3.Distance(firstPosition, secondPosition);
            Vector3 direction = (secondPosition - firstPosition).normalized;

            int drawCount = Mathf.FloorToInt(distance / (sizeNode + distancePerNode));
            for (int j = 1; j <= drawCount; j++)
            {
                Vector3 nextPosition = firstPosition + (direction * j * (sizeNode + distancePerNode));
                Gizmos.DrawWireCube(nextPosition, Vector3.one * sizeNode);
            }
        }
    }
}
