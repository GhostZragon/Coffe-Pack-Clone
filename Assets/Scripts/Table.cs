using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Table : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Vector2Int rowAndCol;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float cellWidth = .25f;
    [SerializeField] private float cellDepth = .25f;
    [SerializeField] private float cameraOffsetZ = .25f;
    
    private Dictionary<Vector2Int, Cell> tableMap = new();
    private bool isRefresh = false;

    [SerializeField] private Bounds bounds;
    
    private void Awake()
    {
        tableMap = new();
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
        }
    }

    private void CreateTable()
    {
        isRefresh = true;

        int count = slots.Count;
        //int row, col;

        //row = rowAndCol.x;
        //col = rowAndCol.y;

        int rows = 4;
        int columns = 4;
        
        float startX = -(columns * (cellWidth + spacing) - spacing) / 2;
        float startZ = -(rows * (cellDepth + spacing) - spacing) / 2;

        for (int i = 0; i < slots.Count; i++)
        {
            if(i >= rows * columns)
            {
                break;
            }
            
            int row = i / columns;
            int col = i % columns;
            
            Debug.Log($"Local Row: {row}, Col: {col}");
            
            float posX = startX + col * (cellWidth + spacing);
            float posZ = startZ + row * (cellDepth + spacing);

            var child = slots[i].transform;
            float currentY = child.localPosition.y;
            // set position and name
            child.localPosition = new Vector3(posX, currentY, posZ);
            child.name = $"Slot; {row} : {col}";
            // Create cell to hold data
            Cell cell = new Cell();
            cell.actualCell = child.gameObject;
            // create table 
            var position = new Vector2Int(row, col);
            tableMap[position] = cell;
            
            bounds.Encapsulate(child.position);
        }
        
        // bounds.Expand(Vector3.one);

        SetCameraToCenterOfTable();
        
        Debug.Log($"Row: {rows}, Col: {columns}");
        Debug.Log("Total count cell: " + tableMap.Count);
    }

    [Button]
    private void SetCameraToCenterOfTable()
    {
        Vector3 centerPosition = transform.position + bounds.center;
        mainCamera.transform.position = 
            new Vector3(centerPosition.x, mainCamera.transform.position.y, centerPosition.z + cameraOffsetZ);
    }
}
[Serializable]
public class Cell
{
    public Vector2Int position;
    public GameObject actualCell;
}