using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _rows = 4;
    [SerializeField] private int _columns = 4;
    [SerializeField] private float _spacing = 1f;
    [SerializeField] private float _cellWidth = 0.25f;
    [SerializeField] private float _cellDepth = 0.25f;

    private Dictionary<Vector2Int, Cell> _cells = new();
    public IReadOnlyDictionary<Vector2Int, Cell> TableMap
    {
        get => _cells;
    }

    private float _startX, _startZ;

    public void InitializeGrid(List<Slot> slots)
    {
        CalculateGridOrigin();
        CreateCells(slots);
    }
    public Cell GetCell(Vector2Int gridPos) => _cells.TryGetValue(gridPos, out var cell) ? cell : null;
    public bool TryGetCell(Vector2Int gridPos, out Cell cell) => _cells.TryGetValue(gridPos, out cell);
    private void CalculateGridOrigin()
    {
        _startX = -(_columns * (_cellWidth + _spacing) - _spacing) / 2;
        _startZ = -(_rows * (_cellDepth + _spacing) - _spacing) / 2;
    }

    private void CreateCells(List<Slot> slots)
    {
        _cells.Clear();
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= _rows * _columns) break;
            
            Vector2Int gridPos = new(i / _columns, i % _columns);
            Slot slot = slots[i];
            
            PositionSlot(slot.transform, gridPos);
            _cells[gridPos] = new Cell(slot);
        }
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
}