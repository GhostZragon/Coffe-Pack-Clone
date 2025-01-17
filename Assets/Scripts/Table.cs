using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audune.Utils.Dictionary;
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

        HandleVisualizeDebugInput();
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

        // if (tableMap.ContainsKey(new Vector2Int(x, z)))
        // {
        //     Debug.Log($"!! You have click in cell {x} {z}", gameObject);
        // }
        // else
        // {
        //     Debug.LogWarning($"You dont have this cell in table, pos{position},x{x}, z{z} ",gameObject);
        // }

        return new Vector2Int(z, x);
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
    public SerializableDictionary<string, List<PriorityTray>> groupOfItems = new();
    public SerializableDictionary<string, int> test;

    public void Checking(Slot slot, string itemId = null)
    {
        var positionOfSlot = TryToGetCell(slot.transform.position);
        int StartX, StartY;
        Debug.Log("================Checking================");
        // groupOfItems.Clear();
        // search for item in current tray
        groupOfItems.Clear();
        var cell = tableMap[positionOfSlot];
        
        if (cell.actualCell.GetTray() == null)
        {
            Debug.Log($"Tại vị trí này, cell đang bị null {positionOfSlot} >???");
            return;
        }
        // reset độ ưu tiên của slot trước
        foreach (var item in nextTimeChecking)
        {
            item.isPlacedSlot = false;
            item.Calculator();
        }
        nextTimeChecking.Clear();
        
        foreach (var itemID in cell.actualCell.GetTray().GetUniqueItemIDs())
        {
            // Init
            if (groupOfItems.ContainsKey(itemID) == false)
            {
                groupOfItems[itemID] = new List<PriorityTray>();
                Debug.Log("Khởi tạo Group of item priority tray");
            }

            // Checking direction
            Debug.Log($"Bắt đầu kiểm tra ItemID {itemID} ở vị trí {positionOfSlot}");
            foreach (var dir in directions)
            {
                StartX = dir.x + positionOfSlot.x;
                StartY = dir.y + positionOfSlot.y;
                Vector2Int checkingPosition = new Vector2Int(StartX, StartY);
                if (!IsValidSlot(StartX, StartY))
                {
                    Debug.Log($"Vị trí kiểm tra không hợp lệ {checkingPosition}");
                    continue;
                }

                Debug.Log($"Vị trí kiểm tra hợp lệ {checkingPosition}");

                var checkingCell = tableMap[checkingPosition];

                if (checkingCell.actualCell.TryGetTray(out Tray checkingTray) && checkingTray.GetCountOfItem(itemID) > 0)
                {
                    NewMethod(checkingTray, itemID);
                }
            }
            Debug.Log("Thêm cell người chơi đã đặt vào");
            NewMethod(cell.actualCell.GetTray(), itemID, true);
        }
    }

    private List<PriorityTray> nextTimeChecking = new();
    
    private void NewMethod(Tray checkingTray, string itemID, bool isCheckingSlot = false)
    {
        PriorityTray priorityTray = new();
        priorityTray.Init(checkingTray, itemID, isCheckingSlot);
       
        groupOfItems[itemID].Add(priorityTray);
        
        if (isCheckingSlot)
            nextTimeChecking.Add(priorityTray);
    }

    public string visualizeItemID;
    private void HandleVisualizeDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Visualize();
        }
    }
    public void Visualize()
    {
        foreach(var item in groupOfItems)
            foreach (var priorityTray in item.Value)
            {
                 priorityTray.Clear();
            }
        
        if (groupOfItems.TryGetValue(visualizeItemID, out var list))
        {
            foreach (var item in list)
            {
                item.RefreshDebugView();
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

[Serializable]
public class PriorityTray
{
    public Tray Tray;
    public string MainItemID;
    public int MainLevel;
    public int SubLevel;
    public bool isPlacedSlot = false;
    private TrayDebugVisualize trayDebugVisualize;
    public void RefreshDebugView()
    {
        if (trayDebugVisualize == null)
        {
            trayDebugVisualize = Tray.GetComponent<TrayDebugVisualize>();
        }
        trayDebugVisualize.Refresh(this);
    }
    
    public void Clear()
    {
        if (trayDebugVisualize == null)
        {
            trayDebugVisualize = Tray.GetComponent<TrayDebugVisualize>();
        }    
        trayDebugVisualize.Refresh(null);
    }

    public void Init(Tray checkingTray, string itemID, bool isCheckingSlot = false)
    {
        Tray = checkingTray;
        MainItemID = itemID;
        isPlacedSlot = isCheckingSlot;
        Calculator();
    }

    public void Calculator()
    {
        int uniqueItemCount = Tray.GetUniqueItemIDs().Count;
        int itemCount = Tray.GetCountOfItem(MainItemID);

        // If the tray is full, add 0; otherwise, add 1
        int trayFullBonus = Tray.CanAddMoreItem() ? 0 : 1;

        // If this slot was just placed by the player, add 1
        int playerPlacedSlotBonus = isPlacedSlot ? 1 : 0;
        
        MainLevel = uniqueItemCount;
        SubLevel = itemCount + trayFullBonus + playerPlacedSlotBonus;
    }
}