using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

public class Table : MonoBehaviour
{
    public static Table Instance;
    
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private List<Slot> slots;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float cellWidth = .25f;
    [SerializeField] private float cellDepth = .25f;
    [SerializeField] private float cameraOffsetZ = .25f;
    
    private Dictionary<Vector2Int, Cell> tableMap = new();
    private bool isRefresh = false;

    private float startX, startZ;
    private float posX, posZ;
    private int rows, columns;
    [SerializeField] private Bounds bounds;

    private readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    
    private void Awake()
    {
        Instance = this;
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

        rows = 4;
        columns = 4;
        
        startX = -(columns * (cellWidth + spacing) - spacing) / 2;
        startZ = -(rows * (cellDepth + spacing) - spacing) / 2;

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

            Debug.Log(GetGridCoordinates(posX,startX,cellWidth,spacing));
            Debug.Log(GetGridCoordinates(posZ,startZ,cellDepth,spacing));

            var child = slots[i].transform;
            float currentY = child.localPosition.y;
            // set position and name
            child.localPosition = new Vector3(posX, currentY, posZ);
            child.name = $"Slot; {row} : {col}";
            // Create cell to hold data
            Cell cell = new Cell();
            cell.actualCell = child.gameObject.GetComponent<Slot>();
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
    int GetGridCoordinates(float posX,  float startX,  float cellWidth, float spacing) {
        // First, offset the position by the grid's start position to get coordinates relative to grid
        float relativeX = posX - startX;
    
        // Calculate column by dividing X position by the size of each grid step (cell + spacing)
        int col = (int)(relativeX / (cellWidth + spacing));
    
        // Calculate row similarly using Z position

        return col;
    }

    public Vector2Int TryToGetCell(Vector3 position)
    {
        int x = GetGridCoordinates(position.x, startX, cellWidth, spacing);
        int z = GetGridCoordinates(position.z, startZ, cellDepth, spacing);

        if (tableMap.ContainsKey(new Vector2Int(x, z)))
        {
            Debug.Log($"!! You have click in cell {x} {z}", gameObject);
        }
        else
        {
            Debug.LogWarning($"You dont have this cell in table, pos{position},x{x}, z{z} ",gameObject);
        }

        return new Vector2Int(x, z);
    }
    
    [Button]
    private void SetCameraToCenterOfTable()
    {
        Vector3 centerPosition = transform.position + bounds.center;
        mainCamera.transform.position = 
            new Vector3(centerPosition.x, mainCamera.transform.position.y, centerPosition.z + cameraOffsetZ);
    }

    List<Tray> emptyTray = new();
    List<Tray> processTray = new();
    private Dictionary<string, List<PriorityTray>> groupOfItems = new();
    public void Checking(Slot slot, string itemId = null)
    {
        var cellOfSlot = TryToGetCell(slot.transform.position);
        int StartX, StartY;
        Debug.Log("================Checking================");
        
        // search for item in current tray
        foreach (var itemID in tableMap[cellOfSlot].actualCell.GetTray().GetUniqueItemIDs())
        {
            // Init
            if (groupOfItems.ContainsKey(itemID) == false)
            {
                groupOfItems[itemID] = new List<PriorityTray>();
                Debug.Log("Khởi tạo Group of item priority tray");
            }
            // Checking direction
            Debug.Log($"Bắt đầu kiểm tra ItemID {itemID} ở vị trí {cellOfSlot}");
            foreach (var dir in directions)
            {
                StartX = dir.x + cellOfSlot.x;
                StartY = dir.y + cellOfSlot.y;
                Vector2Int checkingPosition = new Vector2Int(StartX, StartY);
                if (!IsValidSlot(StartX, StartY))
                {
                    Debug.Log($"Vị trí kiểm tra không hợp lệ {checkingPosition}");
                    continue;
                }
                Debug.Log($"Vị trí kiểm tra hợp lệ {checkingPosition}");
                
                var checkingCell = tableMap[checkingPosition];

                if (checkingCell.actualCell.TryGetTray(out Tray checkingTray))
                {
                    PriorityTray priorityTray = new();
                    
                    int uniqueItemCount = checkingTray.GetUniqueItemIDs().Count;
                    int itemCount = checkingTray.GetCountOfItem(itemID);

                    // If the tray is full, add 0; otherwise, add 1
                    int trayFullBonus = checkingTray.CanAddMoreItem() ? 0 : 1;

                    // If this slot was just placed by the player, add 1
                    int playerPlacedSlotBonus = slot == checkingCell.actualCell ? 1 : 0;

                    priorityTray.Tray = checkingTray;
                    priorityTray.MainItemID = itemID;
                    priorityTray.MainLevel = uniqueItemCount;
                    priorityTray.SubLevel = itemCount + trayFullBonus + playerPlacedSlotBonus;
                  
                    groupOfItems[itemID].Add(priorityTray); ;
                }
            }
        }
        
    }

    private bool IsValidSlot(int checkX,int checkY)
    {
        return checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns;
    }
}


[Serializable]
public class Cell
{
    public Vector2Int position;
    public Slot actualCell;
}


public class PriorityTray
{
    public Tray Tray;
    public string MainItemID;
    public int MainLevel;
    public int SubLevel;
}