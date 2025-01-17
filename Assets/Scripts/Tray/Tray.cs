using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [Header("Settings")]
    [SerializeField] private int index;
    [SerializeField] private int maxItem;
    [Header("Gizmos")] 
    [SerializeField] private Vector3 size;
    [Header("Item settings")]
    public int randomCount;

    private const int SLOT_INDEX = -1;

    public int Index
    {
        get => index;
        set => index = value;
    }

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
    

    public void Add(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            SetStandPosition(item);
        }
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
            if(itemIDs.Contains(item.itemID) == false)
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
    
    private void SetStandPosition(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].name = "Item_" + i;
            item.transform.parent = itemHolder;
            item.transform.position = points[i].transform.position;
            item.transform.SetSiblingIndex(i);
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
        return index == SLOT_INDEX;
    }

    [Button]
    public void RequestItem()
    {
        int count = randomCount > maxItem ? maxItem : Random.Range(1, randomCount);
        for (int i = 0; i < count; i++)
        {
            var item = ItemMananger.Instance.GetNewItem();
            
            Add(item);
        }
    }

    
    
    #region Debug

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


    #endregion Debug


    
}