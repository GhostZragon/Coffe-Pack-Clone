using System.Collections.Generic;
using LitMotion;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _rows = 4;
    [SerializeField] private int _columns = 4;
    [SerializeField] private float _spacing = 1f;
    [SerializeField] private float _cellWidth = 0.25f;
    [SerializeField] private float _cellDepth = 0.25f;
    [SerializeField] private CameraHandler cameraHandler;
    [SerializeField] private CSVImport csvImport;

    [SerializeField] private AudioSource stoneDropSound;
    private Dictionary<Vector2Int, Cell> _cells = new();

    public IReadOnlyDictionary<Vector2Int, Cell> TableMap
    {
        get => _cells;
    }

    private float _startX, _startZ;

    private void Awake()
    {
        cameraHandler = Camera.main.GetComponent<CameraHandler>();
    }


    public void InitializeGrid()
    {
        csvImport.Init();
        CalculateGridOrigin();
        CreateCells();
        cameraHandler.SetupCamera(SlotManager.Instance.transform);
    }

    public Cell GetCell(Vector2Int gridPos) => _cells.TryGetValue(gridPos, out var cell) ? cell : null;
    public bool TryGetCell(Vector2Int gridPos, out Cell cell) => _cells.TryGetValue(gridPos, out cell);

    private void CalculateGridOrigin()
    {
        _startX = -(_columns * (_cellWidth + _spacing) - _spacing) / 2;
        _startZ = -(_rows * (_cellDepth + _spacing) - _spacing) / 2;
    }

    private void CreateCells()
    {
        _cells.Clear();
        _rows = csvImport.maze.GetLength(0);
        _columns = csvImport.maze.GetLength(1);
        
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                int value = csvImport.maze[i, j];
                SlotType slotType = SlotType.Normal;
                switch (value)
                {
                    case 2:
                        slotType = SlotType.Blocking;
                        break;
                }

                if (value == 0) continue;
                var gridPos = new Vector2Int(i, j);
                var slot = SlotManager.Instance.GetSlot(slotType);
                slot.name += $"{i} : {j}";
                PositionSlot(slot.transform, gridPos);
                _cells[gridPos] = new Cell(slot);
            }
        }
        Debug.Log($"level create is {_rows}x{_columns}");
    }
    
    [Button]
    private void TestDropEffect()
    {
        // stoneDropSound.Play();
        // LMotion.Create(0, 1, 0.2f).Bind(x => stoneDropSound.volume = x);
        
        float rowDelayFactor = AnimationManager.Instance.config.gridCfg.rowDelayFactor;
        float columnDelayFactor =  AnimationManager.Instance.config.gridCfg.columnDelayFactor;
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                float delay = (i * rowDelayFactor) + (j * columnDelayFactor);

                var gridPos = new Vector2Int(i, j);
                _cells[gridPos].Slot?.InitEffect(delay);
            }
        }

        // Invoke(nameof(StopSoundWithDelay),rowDelayFactor * _rows);
    }

    private void StopSoundWithDelay()
    {
        LMotion.Create(1, 0, 1).Bind(x => stoneDropSound.volume = x);
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

    public bool IsHaveSlotToDrop()
    {
        foreach (var item in _cells)
        {
            // have slot to drop
            if (item.Value.HasTray == false)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearGrid()
    {
        foreach (var item in _cells)
        {
            if (item.Value.HasSlot)
            {
                Destroy(item.Value.Slot.gameObject);
            }
        }
        _cells.Clear();
    }
}