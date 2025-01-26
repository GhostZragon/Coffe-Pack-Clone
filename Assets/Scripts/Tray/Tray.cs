using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tray : MonoBehaviour
{
    [Header("Stand Point")] [SerializeField]
    private Transform[] points;

    public List<Item> items = new();
    [SerializeField] private Transform pointHolder;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Collider collider;
    [Header("Settings")] [SerializeField] private int index;
    [SerializeField] private int maxItem;
    [Header("Gizmos")] [SerializeField] private Vector3 size;
    [Header("Item settings")] public int randomCount;
    [SerializeField] private Transform trayModel;
    private const int OutsideSlotIndex = -1;


    public int MaxCount
    {
        get => maxItem;
    }

    public int Index
    {
        get => index;
        set => index = value;
    }

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }


    public void Add(Item item, bool isUsingAnimation = true)
    {
        if (items.Count == maxItem)
        {
            Debug.LogWarning("Already have full of item in tray, dont add more",gameObject);
            return;
        }
        
        if (!items.Contains(item))
        {
            items.Add(item);
        }

        // MoveAnimation(items.Count - 1, isUsingAnimation);
        SetStandPosition(isUsingAnimation);
    }
    
    
    public void Remove(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public bool CanAddMoreItem()
    {
        return items.Count < maxItem;
    }

    public List<string> GetUniqueItemIDs()
    {
        List<string> itemIDs = new();
        foreach (var item in items)
        {
            if (itemIDs.Contains(item.itemID) == false)
                itemIDs.Add(item.itemID);
        }

        return itemIDs;
    }
    
    public int GetCountOfItem(string itemID)
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.itemID == itemID)
                count++;
        }

        return count;
    }

    public Item GetFirstOfItem(string itemID)
    {
        foreach (var item in items)
        {
            if (item.itemID == itemID)
                return item;
        }

        return null;
    }


    private void SetStandPosition(bool isUsingAnimation)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].name = "Item_" + i;
            items[i].transform.parent = itemHolder;
            // items[i].transform.position = points[i].transform.position;
            items[i].transform.SetSiblingIndex(i);

            if (isUsingAnimation)
            {
                AnimationManager.Instance.TransferItem(items[i].transform, points[i].position);
            }
            else
            {
                items[i].transform.position = points[i].transform.position;
            }
        }
    }


    [Button]
    public void GoBack()
    {
        if (IsInSlot()) return;

        var stand = TrayManager.instance.GetStandPosition(index);
        transform.position = stand.position;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void SetTrayToSlot()
    {
        index = -1;
    }

    public bool IsInSlot()
    {
        return index == OutsideSlotIndex;
    }

    [Button]
    public void RequestItem()
    {
        int count = randomCount > maxItem ? maxItem : Random.Range(1, randomCount);
        for (int i = 0; i < count; i++)
        {
            if (ItemMananger.Instance == null)
            {
                Debug.LogWarning("Item Manager is null",gameObject);
                return;
            }
            
            var item = ItemMananger.Instance.GetNewItem();
            
            if (item == null)
            {
                Debug.LogWarning("Item get from Item Manager is null",gameObject);
                return;
            }
            Add(item,false);
        }
    }


    #region Debug
#if UNITY_EDITOR
    [Button]
    private void CreatePoint()
    {
        if (points != null)
        {
            foreach (var item in points)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        points = new Transform[maxItem];

        for (int i = 0; i < maxItem; i++)
        {
            var go = new GameObject();
            go.transform.parent = pointHolder.transform;
            go.transform.position = Vector3.zero;
            go.name = "Point_" + i;
            points[i] = go.transform;
        }
    }

    private void OnDrawGizmos()
    {
        DrawPoints();
    }

    private void DrawPoints()
    {
        foreach (var point in points)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(point.transform.position, size);
        }
    }
#endif

    #endregion Debug
}
