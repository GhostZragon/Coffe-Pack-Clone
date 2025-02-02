using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audune.Utils.Dictionary;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;


public partial class Table : MonoBehaviour
{
    public static Table Instance;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float cellWidth = .25f;
    [SerializeField] private float cellDepth = .25f;
    [SerializeField] private float cameraOffsetZ = .25f;

    private bool isRefresh = false;
    private MergeSystem mergeSystem;
    private CameraHandler cameraHandler;

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
        cameraHandler = mainCamera.GetComponent<CameraHandler>();
        
        cameraHandler.ClearBound();
        gridManager.InitializeGrid();
        mergeSystem = new MergeSystem();

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
    }

    public SerializableDictionary<string, List<PriorityTray>> groupOfItems = new();

    public void CheckingMergeSlot(SlotBase slot)
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
            mergeSystem.Merge(item.Value);
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


    [Button]
    private void ClearCurrentTray()
    {
        foreach (var item in gridManager.TableMap)
        {
            item.Value.Slot.PlayClearAnimation();
        }
    }

    public void OnCompleteItem(SlotBase slot)
    {
        var cellPosition = gridManager.WorldToGridPosition(slot.transform.position);
        Vector2Int checkingPosition = Vector2Int.zero;

        foreach (var direction in directions)
        {
            checkingPosition = cellPosition + direction;

            if (gridManager.IsValidGridPosition(checkingPosition))
            {
                var NeighbourCell = gridManager.GetCell(checkingPosition);
                NeighbourCell.Slot?.ActiveSpecialEffect();
            }
        }
    }

    public void ReplaceSlot(SlotBase currentSlot,SlotBase newSlot)
    {
        newSlot.transform.position = currentSlot.transform.position;
        
        var cellPosition = gridManager.WorldToGridPosition(currentSlot.transform.position);
        var cell = gridManager.GetCell(cellPosition);
        cell.SetSlot(newSlot as Slot);
    }
}