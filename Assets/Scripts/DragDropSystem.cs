using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class DragDropSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector2 inputDirection;
    [SerializeField] private float dragSpeed = 5;
    [SerializeField] private Tray selectionObject;
    [SerializeField] private Transform collideObject;
    [SerializeField] private bool isDestroyByClick = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSelectionObject();
    }

    private void HandleSelectionObject()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isDestroyByClick = !isDestroyByClick;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionObject == null)
            {
                if (isDestroyByClick)
                {
                    DeleteTrayInSlot();
                }
                else
                {
                    PickupTray();
                }
            }
            else
            {
                ReleaseTray();
            }
        }

        HandleDragging();
    }

    private void ReleaseTray()
    {
        if (slotObject != null && TryToReleaseTrayInSlot(out Slot slot))
        {
            Debug.Log("Slot is empty and add to slot", slot.gameObject);
            slot.SetEmpty(false);
            slot.Add(selectionObject);

            // logic checking here
            Table.Instance.CheckingMergeSlot(slot);
            //
            TrayManager.instance.Remove(selectionObject);
            TrayManager.instance.TryCreateNextTrays();

            selectionObject = null;
        }
        else
        {
            selectionObject.GoBack();
            selectionObject.EnableCollider();
            selectionObject = null;
            trayObject = null;
        }
    }

    private void DeleteTrayInSlot()
    {
        if (slotObject != null && slotObject.TryGetComponent(out Slot slot))
        {
            slot.RemoveCurrentTray();
        }
    }

    private void HandleDragging()
    {
        Vector3 mousePosition = Input.mousePosition;
        // collide Object using for detect tray and slot
        if (collideObject != null)
        {
            SetWorldPositionByMouse(collideObject, mousePosition);
        }

        // create dragging visual
        if (selectionObject != null && selectionObject.IsInSlot() == false)
        {
            SetWorldPositionByMouse(selectionObject.transform, mousePosition);
            Debug.DrawRay(selectionObject.transform.position, Vector3.down, Color.red);
        }
    }

    private void SetWorldPositionByMouse(Transform moveObject, Vector3 mousePosition)
    {
        // Get the current depth (distance from camera) of the object
        float objectDepth = mainCam.WorldToScreenPoint(moveObject.transform.position).z;

        // Create a screen space position with the correct depth
        Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y, objectDepth);

        // Convert screen position to world position
        Vector3 worldPosition = mainCam.ScreenToWorldPoint(screenPosition);

        // Update object position, maintaining a fixed height
        moveObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
    }

    private bool TryToReleaseTrayInSlot(out Slot slot)
    {
        return slotObject.TryGetComponent(out slot) && slot.IsEmpty();
    }

    private void PickupTray()
    {
        Debug.Log("Try find drag item");
        if (trayObject != null && trayObject.TryGetComponent(out Tray tray) && tray.IsInSlot() == false)
        {
            selectionObject = tray;
            selectionObject.DisableCollider();
            Debug.Log("Finded");
        }
    }

    [SerializeField] private GameObject trayObject;
    [SerializeField] private GameObject slotObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("drag"))
        {
            if (selectionObject != null) return;

            trayObject = other.gameObject;
        }

        // if slot not contain tray, then use it
        if (other.CompareTag("slot"))
        {
            slotObject = other.gameObject;
            if (slotObject.TryGetComponent(out Slot slot) && slot.IsEmpty())
            {
                slot.OnSelect();
                Table.Instance.TryToGetCell(slot.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("drag") && other.gameObject == trayObject)
        {
            trayObject = null;
        }

        // if Collider Object trigger is the current, Call UnSelect()
        if (other.CompareTag("slot") && other.gameObject == slotObject)
        {
            if (slotObject != null && slotObject.TryGetComponent(out Slot slot))
            {
                slot.UnSelect();
            }

            slotObject = null;
        }
    }
}