using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
public class DragDropSystem : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private bool isTrigger = false;
    [SerializeField] private Vector3 triggerPosition;
    [SerializeField] private LayerMask dragLayerMask;
    [SerializeField] private LayerMask slotLayerMask;

    [SerializeField]  private Tray selectionObject;
    [SerializeField]  private Slot currentSlot;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        isTrigger = InputManager.Instance.IsTrigger();
        triggerPosition = InputManager.Instance.GetTouchPosition();
        HandleTrigger();
        
        Dragging();
    }


    private void SlotChecking()
    {
        if (TryRaycast(triggerPosition, slotLayerMask, out var hit))
        {
            if (hit.collider.TryGetComponent(out Slot newSlot))
            {
                if (currentSlot != null && newSlot != null)
                {
                    currentSlot.UnSelect();
                }

                currentSlot = newSlot;

                if (newSlot.CanPlacedTray())
                {
                    newSlot.OnSelect();
                }
            }
        }
        else if (currentSlot != null)
        {
            // Clear slot highlight when not over any slot
            currentSlot.UnSelect();
            currentSlot = null;
        }
    }

    private void HandleTrigger()
    {
        if (isTrigger)
        {
            if (TryRaycast(triggerPosition, dragLayerMask, out var hit) && selectionObject == null)
            {
                if (hit.collider.TryGetComponent(out Tray tray) && !tray.IsInSlot())
                {
                    selectionObject = tray;
                    selectionObject.DisableCollider();
                }
            }
            SlotChecking();
        }
        else
        {
            if (selectionObject != null)
            {
                
                if (TryRaycast(triggerPosition, slotLayerMask, out var hit))
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
                    selectionObject.GoBack();
                    selectionObject = null;
                }
            }
            if (currentSlot != null)
            {
                currentSlot.UnSelect();
                currentSlot = null;
            }
        }
    }
    private RaycastHit[] raycastHits = new RaycastHit[1];

    bool TryRaycast(Vector3 position, LayerMask mask, out RaycastHit hit)
    {
        var ray = camera.ScreenPointToRay(position);
        int count = Physics.RaycastNonAlloc(ray, raycastHits, 100, mask);
        hit = count > 0 ? raycastHits[0] : default;
        return count > 0;
    }
    
    private void Dragging()
    {
        if (selectionObject != null)
        {
            SetWorldPositionByMouse(selectionObject.transform,triggerPosition);
        }
    }
    private readonly Vector3 workingPosition = new Vector3(); // Reuse this vector

    private void SetWorldPositionByMouse(Transform moveObject, Vector3 mousePosition)
    {
        // Get the current depth (distance from camera) of the object
        float objectDepth = camera.WorldToScreenPoint(moveObject.transform.position).z;

        // Create a screen space position with the correct depth
        Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y, objectDepth);

        // Convert screen position to world position
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);

        // Update object position, maintaining a fixed height
        moveObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
    }
}