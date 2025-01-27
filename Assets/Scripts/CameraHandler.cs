using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Bounds levelBounds;

    private void Awake()
    {
        levelBounds = new();
    }

    public void SetupBound(Transform boundObj)
    {
        levelBounds.Encapsulate(boundObj.position);
    }

    public void ClearBound()
    {
        levelBounds = new Bounds();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(levelBounds.center,levelBounds.size);
    }
}
