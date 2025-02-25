using System;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Table : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slot slotPrefab;

    public Action OnProcressComplete;
    
    public MergeSystem mergeSystem;

    private static Table Instance;
    

    [SerializeField] private GridManager gridManager;

    private static readonly Vector2Int[] directions =
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
        mergeSystem = new MergeSystem(this,gridManager);
    }



    public void DestroyBlockingSlotAround(SlotBase slot)
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

    public int x, y;
    [Button]
    public void IsValidSlot()
    {
        Debug.Log("Is Valid: "+gridManager.IsValidGridPosition(new Vector2Int(x,y)));
    }

    private void ProcressComplete()
    {
        OnProcressComplete();
    }

    private int itemMoveCount = 0;

    public static void AddItemMoving() => Instance.itemMoveCount++;
    public static void RemoveItemMoving() => Instance.itemMoveCount--;
    
    
    private bool IsAllItemMoveDone()
    {
        return itemMoveCount == 0;
    }
}