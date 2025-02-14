using System.Collections;
using System.Collections.Generic;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
    [SerializeField] private int _rows = 4;
    [SerializeField] private int _columns = 4;
    [SerializeField] private float _spacing = 1f;
    [SerializeField] private float _cellWidth = 0.25f;
    [SerializeField] private float _cellDepth = 0.25f;
    [SerializeField] private CameraHandler cameraHandler;
    [SerializeField] private AlignCamera alignCamera;
    [SerializeField] private CSVImport csvImport;

    [SerializeField] private DropDownEffect dropDownEffect;
    private Dictionary<Vector2Int, Cell> _cells = new();
    public IReadOnlyDictionary<Vector2Int, Cell> TableMap
    {
        get => _cells;
    }

    private float _startX, _startZ;

    private void Awake()
    {
        cameraHandler = Camera.main.GetComponent<CameraHandler>();
        alignCamera = GetComponent<AlignCamera>();
    }


    public void InitializeGrid()
    {
        csvImport.Init();
        CalculateGridOrigin();
        SettingBeforeCreateCells();
        CreateCells();
        cameraHandler.SetupCamera(SlotManager.Instance.transform);
        alignCamera.UpdateBound();
    }

    public Cell GetCell(Vector2Int gridPos) => _cells.TryGetValue(gridPos, out var cell) ? cell : null;
    public bool TryGetCell(Vector2Int gridPos, out Cell cell) => _cells.TryGetValue(gridPos, out cell);

    private void CalculateGridOrigin()
    {
        _startX = -(_columns * (_cellWidth + _spacing) - _spacing) / 2;
        _startZ = -(_rows * (_cellDepth + _spacing) - _spacing) / 2;
    }

    private void SettingBeforeCreateCells()
    {
        _cells.Clear();
        _rows = csvImport.maze.GetLength(0);
        _columns = csvImport.maze.GetLength(1);
        dropDownEffect.Setup(_cells, _rows, _columns);
    }
    
    private void CreateCells()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                int value = csvImport.maze[i, j];

                if (value == 0) continue;
               
                var gridPos = new Vector2Int(i, j);
                var slot = SlotManager.Instance.GetSlot(GetSlotTypeByValue(value));
                slot.name += $"{i} : {j}";
                PositionSlot(slot.transform, gridPos);
                _cells[gridPos] = new Cell(slot);
                slot.SetSize(_cellWidth,_cellDepth);
            }
        }

        Debug.Log($"level create is {_rows}x{_columns}");
    }

    private SlotType GetSlotTypeByValue(int value)
    {
        switch (value)
        {
            case 2:
                return SlotType.Blocking;
                break;
        }

        return SlotType.Normal;
    }

    [Button]
    private void TestDropEffect()
    {
        dropDownEffect?.Play();
    }


    private void PositionSlot(Transform slotTransform, Vector2Int gridPos)
    {
        Vector3 worldPos = new(
            _startX + gridPos.y * (_cellWidth + _spacing),
            slotTransform.localPosition.y,
            _startZ + gridPos.x * (_cellDepth + _spacing)
        );
        slotTransform.localPosition = worldPos;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition.z - _startZ) / (_cellDepth + _spacing)),
            Mathf.FloorToInt((worldPosition.x - _startX) / (_cellWidth + _spacing))
        );
    }

    public bool IsValidGridPosition(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < _rows &&
               gridPos.y >= 0 && gridPos.y < _columns;
    }

    public void SetLevelData(TextAsset LevelCsv)
    {
        csvImport.mapCSV = LevelCsv;
    }

    public bool IsFullOfSpace()
    {
        foreach (var item in _cells)
        {
            // have slot to drop
            if (item.Value.HasTray == false)
            {
                return false;
            }
        }

        return true;
    }

    public void ClearGrid()
    {
        foreach (var item in _cells)
        {
            item.Value.ClearTrayAndSlot();
        }

        _cells.Clear();
    }

    private void OnDrawGizmos()
    {
        if (_cells == null || _cells.Count == 0) return;

        Gizmos.color = Color.green;

        foreach (var cell in _cells)
        {
            Vector3 center = new Vector3(
                _startX + cell.Key.y * (_cellWidth + _spacing),
                0,
                _startZ + cell.Key.x * (_cellDepth + _spacing)
            );

            Vector3 size = new Vector3(_cellWidth, 0.1f, _cellDepth); // Độ dày nhỏ để dễ nhìn

            Gizmos.DrawWireCube(center, size);
        }
    }

}