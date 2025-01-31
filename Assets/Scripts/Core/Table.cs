using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audune.Utils.Dictionary;
using NUnit.Framework;
using Sirenix.OdinInspector;
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

    // private Dictionary<Vector2Int, Cell> tableMap = new();
    private bool isRefresh = false;

    private CameraHandler cameraHandler;
    // private float startX, startZ;
    // private float posX, posZ;
    // private int rows, columns;
    [SerializeField] private GridManager gridManager;

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
        mainCamera = Camera.main;
        SetupSlots();
        
        gridManager.InitializeGrid(slots);
        

        cameraHandler = mainCamera.GetComponent<CameraHandler>();
        cameraHandler.ClearBound();
        cameraHandler.SetupBound(slots);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MergeGroupOfItems();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearCurrentTray();
        }

        HandleVisualizeDebugInput();
    }

    private void SetupSlots()
    {
        isRefresh = true;
        for (int i = 0; i < slots.Count; i++)
        {
            var child = slots[i].transform;
            var slot = child.gameObject.GetComponent<Slot>();
            slot.PlacedCallback = CheckingMergeSlot;
        }
    }


    public SerializableDictionary<string, List<PriorityTray>> groupOfItems = new();

    private void CheckingMergeSlot(Slot slot)
    {
        var gridPos = gridManager.WorldToGridPosition(slot.transform.position);
        Debug.Log("================Checking================");

        groupOfItems.Clear();
        var cell = gridManager.GetCell(gridPos);

        if (cell.Tray == null)
        {
            Debug.Log($"Tại vị trí này, cell đang bị null {gridPos} >???");
            return;
        }

        // reset độ ưu tiên của slot trước
        ClearPreviousPriorityData();

        FindNeighborItems(cell, gridPos);

        Debug.Log("End of merge");
        Invoke(nameof(MergeGroupOfItems), .2f);
        // MergeGroupOfItems();
    }

    private void FindNeighborItems(Cell cell, Vector2Int gridPos)
    {
        foreach (var itemID in cell.Tray.GetUniqueItemIDs())
        {
            if (groupOfItems.ContainsKey(itemID) == false)
            {
                groupOfItems[itemID] = new List<PriorityTray>();
                Debug.Log("Khởi tạo Group of item priority tray");
            }

            FindPotentialNeighbors(gridPos, itemID);

            InitPriorityTray(cell.Tray, itemID, true);

            UpdatePrioritiesForGroup(itemID);
        }
    }

    private void FindPotentialNeighbors(Vector2Int centerPosition, string itemID)
    {
        foreach (var direction in directions)
        {
            var neighbourPosition = new Vector2Int(direction.x + centerPosition.x, direction.y + centerPosition.y);
            if (ShouldProcessNeighbor(neighbourPosition, out var tray) && CanProcessTray(tray, itemID))
            {
                InitPriorityTray(tray, itemID);
            }
        }
    }

    private bool CanProcessTray(Tray tray, string itemID)
    {
        return tray.GetCountOfItem(itemID) > 0;
    }

    private bool ShouldProcessNeighbor(Vector2Int gridPos, out Tray tray)
    {
        tray = null;
        if (gridManager.IsValidGridPosition(gridPos) &&
            gridManager.TryGetCell(gridPos, out var cell) && cell.HasTray)
        {
            tray = cell.Tray;
        }

        return tray != null;
    }

    private void ClearPreviousPriorityData()
    {
        foreach (var item in previusPriorityPlaced)
        {
            item.isPlacedSlot = false;
            item.Calculator();
        }

        previusPriorityPlaced.Clear();
    }

    private List<PriorityTray> previusPriorityPlaced = new();

    private void InitPriorityTray(Tray checkingTray, string itemID, bool isCheckingSlot = false)
    {
        PriorityTray priorityTray = new();
        priorityTray.Init(checkingTray, itemID, isCheckingSlot);

        groupOfItems[itemID].Add(priorityTray);

        if (isCheckingSlot)
            previusPriorityPlaced.Add(priorityTray);
    }



   
    private void UpdatePrioritiesForGroup(string itemID)
    {
        if (groupOfItems.TryGetValue(itemID, out var list))
        {
            foreach (var item in list)
            {
                item.Calculator();
            }
            list.Sort();
        }
    }


    [Header("Debug")] public string visualizeItemID;

    private void HandleVisualizeDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Visualize();
        }
    }

    private void Visualize()
    {
        foreach (var item in groupOfItems)
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

    [Button]
    private void MergeGroupOfItems()
    {
        StartCoroutine(MergeGroupOfItemsCoroutine());
    }

    private IEnumerator MergeGroupOfItemsCoroutine()
    {
        foreach (var item in groupOfItems)
        {
            Debug.Log("Working on: " + item.Key);
            Merge(item.Value);
            yield return new WaitForSeconds(0.1f);
            CalculatorAllGroupPriorityAndSort();
        }

        yield return new WaitForSeconds(.5f);
        ClearCurrentTray();
    }

    private void CalculatorAllGroupPriorityAndSort()
    {
        foreach (var item in groupOfItems)
        {
            UpdatePrioritiesForGroup(item.Key);
        }
    }
    
    private void Merge(List<PriorityTray> sources)
    {
        if (sources.Count < 2)
        {
            Debug.Log("Danh sách tray này không đủ để merge", gameObject);
            return;
        }

        string mainMergeItemID = sources[0].MainItemID;

        Queue<Tray> queueTray = new();

        foreach (var priorityTray in sources)
        {
            queueTray.Enqueue(priorityTray.Tray);
        }

        Tray currentSource = queueTray.Dequeue();
    
        while (queueTray.Count > 0)
        {
            Tray nextTray = queueTray.Peek(); // Only peek, don't dequeue yet
        
            // If current source is full, move to next source
            if (!currentSource.CanAddMoreItem())
            {
                currentSource = queueTray.Dequeue();
                continue;
            }

            // If next tray has no relevant items, skip it
            if (nextTray.GetCountOfItem(mainMergeItemID) == 0)
            {
                queueTray.Dequeue();
                continue;
            }

            // Transfer item
            var item = nextTray.GetFirstOfItem(mainMergeItemID);
            if (item != null)
            {
                nextTray.Remove(item);
                currentSource.Add(item);
            
                // If next tray is empty, remove it from queue
                if (nextTray.GetCountOfItem(mainMergeItemID) == 0)
                {
                    queueTray.Dequeue();
                }
            }
        }
    }

    [Button]
    private void ClearCurrentTray()
    {
        foreach (var item in gridManager.TableMap)
        {
            item.Value.Slot.PlayClearAnimation();
        }
    }
}