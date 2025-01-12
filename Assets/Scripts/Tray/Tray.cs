using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [Header("Stand Point")]
    [SerializeField] private Transform[] points;
    [SerializeField] private List<Item> items = new();
    [SerializeField] private Transform pointHolder;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Collider collider;
    [Header("Settings")]
    [SerializeField] private int index;
    [SerializeField] private int maxItem;
    [Header("Gizmos")]
    [SerializeField] private Vector3 size;

    public int Index { get => index; set => index = value; }

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public void Add(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            SetStandPosition(item, items.Count);
        }
    }

    private void SetStandPosition(Item item, int index)
    {
        item.name = "Item_" + index;
        item.transform.parent = itemHolder;
        item.transform.position = points[index].transform.position;
    }

    public void Remove(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    [Button]
    public void GoBack()
    {
        var stand = TrayManager.instance.GetStandPosition(index);
        if(stand)
            transform.position = stand.position;
    }

    #region  Debug

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

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void SetToSlot()
    {
        index = -1;
    }
    #endregion Debug
}
