using System;
using System.Collections.Generic;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class GridManager
{
    [Serializable]
    private class DropDownEffect
    {
        public enum DropType
        {
            Zigzag,
            Wave
        }

        private int _rows = 0;
        private int _columns = 0;
        private Dictionary<Vector2Int, Cell> _cells;
        
        [SerializeField] private DropType dropType;
        
        public void Setup(Dictionary<Vector2Int, Cell> cells, int rows, int columns)
        {
            _cells = cells;
            _columns = columns;
            _rows = rows;
            if (_rows % 2 != 0 && _columns % 2 != 0)
            {
                dropType = DropType.Wave;
            }
            else
            {
                dropType = DropType.Zigzag;
            }
        }
        
        public void Play()
        {
            switch (dropType)
            {
                case DropType.Wave:
                    DropWaveEffect();
                    break;
                case DropType.Zigzag:
                    ZigzagEffect();
                    break;
            }
        }

        [Button]
        private void ZigzagEffect()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    float delay = (i + j) * 0.1f; // Delay tăng dần theo đường chéo
                    _cells[new Vector2Int(i, j)].Slot?.InitEffect(delay, GetEase(DropType.Zigzag));
                }
            }
        }

        [Button]
        private void DropWaveEffect()
        {
            Vector2Int center = new Vector2Int(_rows / 2, _columns / 2);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    float distance = Vector2Int.Distance(new Vector2Int(i, j), center);
                    float delay = distance * 0.1f; // Delay theo khoảng cách

                    _cells[new Vector2Int(i, j)].Slot?.InitEffect(delay, GetEase(DropType.Wave));
                }
            }
        }

        private Ease GetEase(DropType dropType)
        {
            switch (dropType)
            {
                case DropType.Wave:
                    return AnimationManager.Instance.config.slotCfg.dropDownEaseWave;
                case DropType.Zigzag:
                    return AnimationManager.Instance.config.slotCfg.dropDownEaseZigzag;
            }

            return Ease.Linear;
        }

       
    }
}