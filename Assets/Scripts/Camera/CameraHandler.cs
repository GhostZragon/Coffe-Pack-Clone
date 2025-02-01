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

    public void SetupBound(Slot slot)
    {
        levelBounds.Encapsulate(slot.transform.position);

        // AdjustCameraToFitBounds();

    }

    private void Update()
    {
        transform.position = new Vector3(levelBounds.center.x, transform.position.y, levelBounds.center.z);
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
