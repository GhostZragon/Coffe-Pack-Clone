using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Bounds levelBounds;
    private Camera mainCamera;
    public float padding = 1;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        levelBounds = new();
    }

    public void SetupCamera(Transform container)
    {
        var bound = GetLevelBounds(container);
        AdjustCamera(bound);
    }
    
    Bounds GetLevelBounds(Transform container)
    {
        Bounds bounds = new Bounds();
        foreach (Transform tile in container) // levelTiles là danh sách các ô
        {
            bounds.Encapsulate(tile.position);
        }
        return bounds;
    }
    
    void AdjustCamera(Bounds levelBounds)
    {
        Camera cam = Camera.main;
    
        float levelWidth = levelBounds.size.x;
        float levelHeight = levelBounds.size.y;

        float screenRatio = (float)Screen.width / Screen.height;
        float targetHeight = levelWidth / screenRatio; // Đảm bảo vừa chiều ngang

        float fov = cam.fieldOfView * Mathf.Deg2Rad; // Chuyển FOV sang radian

        float cameraAngle = 50 * Mathf.Deg2Rad; // Góc nghiêng của camera
        float cameraHeight = 25; // Luôn giữ y = 25

        // Tính khoảng cách theo trục Z
        float distance = Mathf.Max(levelHeight, targetHeight) * 0.5f / Mathf.Tan(fov * 0.5f);

        // Điều chỉnh khoảng cách theo góc nghiêng của camera
        float zOffset = distance / Mathf.Cos(cameraAngle);
    
        // Đặt vị trí camera
        cam.transform.position = new Vector3(levelBounds.center.x, cameraHeight, levelBounds.center.z - zOffset / 1.7f);

        // Xoay camera đúng góc mong muốn
        cam.transform.rotation = Quaternion.Euler(50, 0, 0);
    }

    private void Update()
    {
        // transform.position = new Vector3(levelBounds.center.x, transform.position.y, levelBounds.center.z);
    }

    public void ClearBound()
    {
        levelBounds = new Bounds();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(levelBounds.center,levelBounds.size);
    }   
    
    public void AdjustCameraToFitBounds()
    {
        if (mainCamera == null || !mainCamera.orthographic) return;

        var mapSize = levelBounds.size;
        
        // Get the screen aspect ratio
        float screenRatio = (float)Screen.width / Screen.height;
        
        // Calculate the required orthographic size based on map dimensions
        float boundsWidth = mapSize.x + (padding * 2);
        float boundsHeight = mapSize.y + (padding * 2);
        
        // Calculate the orthographic size needed for both width and height
        float orthographicSizeForWidth = boundsWidth / (2f * screenRatio);
        float orthographicSizeForHeight = boundsHeight / 2f;
        
        // Use the larger value to ensure the entire map is visible
        mainCamera.orthographicSize = Mathf.Max(orthographicSizeForWidth, orthographicSizeForHeight);
        
        // Center the camera on the map
        Vector3 centerPosition = levelBounds.center;
        centerPosition.x = mapSize.x / 2f;
        centerPosition.y = mapSize.y / 2f;
        mainCamera.transform.position = new Vector3(centerPosition.x, centerPosition.y, mainCamera.transform.position.z);
    }
}
