using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Vector2Int rowAndCol;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float cellWidth = .25f;
    [SerializeField] private float cellDepth = .25f;
    private Dictionary<Vector2Int, Cell> tableMap = new();
    private bool isRefresh = false;
    private void Awake()
    {
        CreateTable();
    }

    [Button]
    private void Refresh()
    {
        isRefresh = false;
    }

    private void Update()
    {
        if(isRefresh == false)
        {
        CreateTable();
            isRefresh = true;
        }
    }

    private void CreateTable()
    {
        int count = slots.Count;
        //int row, col;

        //row = rowAndCol.x;
        //col = rowAndCol.y;

        int rows = 4;
        int columns = 4;
        
        float startX = -(columns * (cellWidth + spacing) - spacing) / 2;
        float startZ = -(rows * (cellDepth + spacing) - spacing) / 2;

        Vector3 centerPosition = transform.position;

        for (int i = 0; i < slots.Count; i++)
        {
            if(i >= rows * columns)
            {
                break;
            }
            int row = i / columns;
            int col = i % columns;
            var child = slots[i].transform;
            float posX = startX + col * (cellWidth + spacing);
            float posZ = startZ + row * (cellDepth + spacing);

            float currentY = child.localPosition.y;
            child.localPosition = new Vector3(posX, currentY, posZ);
        }

    }
}
[Serializable]
public class Cell
{
    public Vector2Int position;
    public GameObject actualCell;
}