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


    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    private void HandleSelectionObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionObject == null)
            {
                PickupTray();
            }
            else
            {
                selectionObject.GoBack();
                selectionObject.EnableCollider();
                selectionObject = null;
            }
        }


        if (selectionObject != null)
        {
            Vector3 mousePosition = Input.mousePosition;

            // Get the current depth (distance from camera) of the object
            float objectDepth = mainCam.WorldToScreenPoint(selectionObject.transform.position).z;

            // Create a screen space position with the correct depth
            Vector3 screenPosition = new Vector3(mousePosition.x, mousePosition.y, objectDepth);

            // Convert screen position to world position
            Vector3 worldPosition = mainCam.ScreenToWorldPoint(screenPosition);

            // Update object position, maintaining a fixed height
            selectionObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);

            
            Debug.DrawRay(selectionObject.transform.position, Vector3.down, Color.red);
           
            CastSlotHandle();
        }
    }

    [SerializeField] private GameObject selectionSlot;

    private void CastSlotHandle()
    {
        var rayHitSlot = new Ray(selectionObject.transform.position, Vector3.down);

        Physics.Raycast(rayHitSlot,out RaycastHit raycastHit);
        if (raycastHit.collider != null && raycastHit.collider.CompareTag("slot"))
        {
            var hitSlot = raycastHit.collider.gameObject;
            if (selectionSlot != null)
            {
                // if new slot different to current slot
                // then replace that
                if (!Equals(selectionSlot, hitSlot))
                {
                    Debug.Log($"Replace {selectionSlot.name} by {hitSlot.name}");
                    selectionSlot = hitSlot;
                }
                
                
            }
            else
            {
                // update current slot
                Debug.Log($"Replace {selectionSlot?.name} by {hitSlot.name}");
                selectionSlot = hitSlot;
            }
        }
        else
        {
            Debug.Log($"Release exit slot");
            selectionSlot = null;
        }

    }
    
    private void ReleaseTrayInToSlot(RaycastHit hit)
    {
        Debug.Log(hit.collider.name, hit.collider.gameObject);
        // it hit is slot then check can put try into slot
        if (hit.collider.TryGetComponent(out Slot slot) && slot.IsEmpty())
        {
            Debug.Log("Slot is empty and add to slot", slot.gameObject);
            selectionObject.SetToSlot();
            slot.SetEmpty(true);
            slot.Add(selectionObject);
            // set position
            selectionObject.transform.position = slot.transform.position;
            // logic checking here

            //
        }
    }

    private void PickupTray()
    {
        var hit = CastRay();
        Debug.Log("Try find drag item");
        if (hit.collider != null && hit.collider.CompareTag("drag"))
        {
            if (hit.collider.TryGetComponent(out Tray tray))
            {
                selectionObject = tray;
                selectionObject.DisableCollider();
            }

            Debug.Log("Finded");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleSelectionObject();
    }

    private RaycastHit CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Cast ray from camera through mouse position
        Physics.Raycast(ray, out hit, Mathf.Infinity);

        return hit;
    }

   
}