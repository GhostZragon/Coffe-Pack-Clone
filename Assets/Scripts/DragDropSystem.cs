
using UnityEngine;
public class DragDropSystem : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;
    [SerializeField] private Vector3 draggingPosition;
    [SerializeField] private LayerMask dragLayerMask;
    [SerializeField] private LayerMask slotLayerMask;

    [SerializeField]  private Tray selectionObject;
    [SerializeField]  private Slot currentSlot;

    private enum DragState { Idle, Dragging }
    private DragState currentState = DragState.Idle;
    
    private Camera cachedCamera;
    
    private void Awake()
    {
        cachedCamera = Camera.main;
    }

    private void Update()
    {
        isDragging = InputManager.Instance.IsTrigger();
        draggingPosition = InputManager.Instance.GetTouchPosition();
    
        HandleTrigger();
        Dragging();
        
    }

    
    private void SlotIsOverCursor()
    {
        if (TryRaycast(draggingPosition, slotLayerMask, out var hit))
        {
            if (!hit.collider.TryGetComponent(out Slot newSlot)) return;
           
            currentSlot?.UnSelect();

            currentSlot = newSlot;

            if (newSlot.CanPlacedTray())
            {
                newSlot.OnSelect();
            }
            return;
        }
        
        ClearStateOfSlot();
    }

    private void HandleTrigger()
    {
        
        // in case i need to implement duo tray or triple
        // do it in releaseTrayInSlot
        if (isDragging)
        {
            Pickup();
            SlotIsOverCursor();
        }
        else
        {
            ReleaseTrayInSlot();
            // reset trigger when hold back
            ClearStateOfSlot();
        }
    }
    
    private void ClearStateOfSlot()
    {
        if (currentSlot == null) return;
        // Clear slot highlight when not over any slot
        currentSlot.UnSelect();
        currentSlot = null;
    }
    
    private void ReleaseTrayInSlot()
    {
        if (selectionObject == null) return;

        if (TryRaycast(draggingPosition, slotLayerMask, out var hit))
        {
            if (hit.collider.TryGetComponent(out Slot slot) && slot.CanPlacedTray())
            {
                slot.Add(selectionObject);
                selectionObject = null;
            }
        }
        else
        {
            selectionObject.EnableCollider();
            selectionObject.SetTrayToOriginalPosition();
            selectionObject = null;
        }
    }

    private void Pickup()
    {
        if (!TryRaycast(draggingPosition, dragLayerMask, out var hit) || selectionObject != null) return;

        if (!hit.collider.TryGetComponent(out Tray tray) || tray.IsInSlot()) return;
       
        selectionObject = tray;
        selectionObject.DisableCollider();
    }

    private RaycastHit[] raycastHits = new RaycastHit[1];

    bool TryRaycast(Vector3 position, LayerMask mask, out RaycastHit hit)
    {
        var ray = cachedCamera.ScreenPointToRay(position);
        int count = Physics.RaycastNonAlloc(ray, raycastHits, 100, mask);
        hit = count > 0 ? raycastHits[0] : default;
        return count > 0;
    }
    
    private void Dragging()
    {
        if (selectionObject != null)
        {
            SetWorldPositionByMouse(selectionObject.transform,draggingPosition);
        }
    }

    private void SetWorldPositionByMouse(Transform moveObject, Vector3 mousePosition)
    {
        // Get the current depth (distance from camera) of the object
        float objectDepth = cachedCamera.WorldToScreenPoint(moveObject.transform.position).z;

        // Create a screen space position with the correct depth
        Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y, objectDepth);

        // Convert screen position to world position
        Vector3 worldPosition = cachedCamera.ScreenToWorldPoint(screenPosition);

        // Update object position, maintaining a fixed height
        moveObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
    }
}