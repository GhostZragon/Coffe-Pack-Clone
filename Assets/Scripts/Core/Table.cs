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
    public MergeSystem mergeSystem;

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

    public int x, y;
    [Button]
    public void IsValidSlot()
    {
        Debug.Log("Is Valid: "+gridManager.IsValidGridPosition(new Vector2Int(x,y)));
    }
}