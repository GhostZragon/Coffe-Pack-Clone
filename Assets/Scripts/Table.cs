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

    [Serializable]
    public class VisitedObject
    {
        public string itemID;
        public List<Vector2Int> positions;
    }
    [SerializeField] private List<VisitedObject> visitedItems = new List<VisitedObject>();

public void Checking(Slot slot, string itemId = null)
{
    // Khởi tạo lại danh sách khi bắt đầu một lượt kiểm tra mới
    if (itemId == null)
    {
        visitedItems.Clear();
    }

    var checkingCell = TryToGetCell(slot.transform.position);
    Vector2Int currentPos = new Vector2Int(checkingCell.x, checkingCell.y);

    // Kiểm tra xem vị trí này đã được thăm với item hiện tại chưa
    if (itemId != null)
    {
        var currentItemPath = GetOrCreateVisitedObject(itemId);
        if (currentItemPath.positions.Contains(currentPos))
        {
            return;
        }
        // Thêm vị trí hiện tại vào danh sách của item
        currentItemPath.positions.Add(currentPos);
        Debug.Log($"Thêm vị trí ({currentPos.x}, {currentPos.y}) cho item {itemId}");
    }

    foreach (var dir in directions)
    {
        int checkX = dir.x + checkingCell.x;
        int checkY = dir.y + checkingCell.y;
   
        if (IsValidSlot(checkX, checkY))
        {
            Vector2Int checkingPosition = new Vector2Int(checkX, checkY);
            var tableSlot = tableMap[checkingPosition].actualCell;

            if (tableSlot.IsEmpty())
            {
                Debug.Log($"Ô ({checkX}, {checkY}) trống, bỏ qua");
                continue;
            }

            // Trường hợp chưa có itemId cụ thể
            if (itemId == null)
            {
                foreach (var item in slot.GetTray().items)
                {
                    if (tableSlot.GetTray().IsContainItem(item.itemID))
                    {
                        Debug.Log($"Bắt đầu kiểm tra cho item {item.itemID} tại ({checkX}, {checkY})");
                        Checking(tableSlot, item.itemID);
                    }
                }
            }
            // Trường hợp đã có itemId cụ thể
            else if (tableSlot.GetTray().IsContainItem(itemId))
            {
                Debug.Log($"Tiếp tục kiểm tra item {itemId} tại ({checkX}, {checkY})");
                Checking(tableSlot, itemId);
            }
        }
    }

    Processing();
}

// Phương thức hỗ trợ để lấy hoặc tạo mới VisitedObject cho một item
private VisitedObject GetOrCreateVisitedObject(string itemId)
{
    // Tìm VisitedObject cho itemId trong danh sách
    var visitedObject = visitedItems.Find(v => v.itemID == itemId);
    
    // Nếu chưa có, tạo mới và thêm vào danh sách
    if (visitedObject == null)
    {
        visitedObject = new VisitedObject
        {
            itemID = itemId,
            positions = new List<Vector2Int>()
        };
        visitedItems.Add(visitedObject);
    }
    
    return visitedObject;
}

private void Processing()
{
    foreach (var item in visitedItems)
    {
        List<Cell> cells = new List<Cell>();
        foreach (var pos in item.positions)
        {
            cells.Add(tableMap[pos]);
        }

        while (cells.Count > 0)
        {
            Cell source = cells[0];
            cells.RemoveAt(0);
            MergeToSource(source, cells, item.itemID);
        }
    }
}

private void MergeToSource(Cell source, List<Cell> anothers,string itemID)
{
    var sourceTray = source.actualCell.GetTray();
    int index = 0;
    while (sourceTray.CanAddMoreItem() && index < anothers.Count)
    {
        var tempTray = anothers[index].actualCell.GetTray();

        if (tempTray.IsContainItem(itemID))
        {
            var item = tempTray.GetFirstItem(itemID);
            tempTray.items.Remove(item);
            sourceTray.Add(item);
        }

        index++;
    }
}
// Phương thức tiện ích để kiểm tra các vị trí khả dụng cho một item cụ thể
public List<Vector2Int> GetAvailablePositionsForItem(string itemId)
{
    var visitedObject = visitedItems.Find(v => v.itemID == itemId);
    return visitedObject?.positions ?? new List<Vector2Int>();
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