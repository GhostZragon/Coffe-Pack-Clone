using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audune.Utils.Dictionary;
using UnityEngine;

public partial class Table 
{
    public class MergeSystem
    {
        private readonly SerializableDictionary<string, List<TrayMergeCandidate>> _mergeableItemGroups;
        private readonly GridManager _gridManager;
        private readonly Table _table;
        private readonly List<TrayMergeCandidate> _lastProcessedCandidates;

        public MergeSystem(Table table, GridManager gridManager)
        {
            _table = table;
            _gridManager = gridManager;
            _mergeableItemGroups = new SerializableDictionary<string, List<TrayMergeCandidate>>();
            _lastProcessedCandidates = new List<TrayMergeCandidate>();
        }

        public void TryMergeAtSlot(SlotBase slot)
        {
            var gridPosition = _gridManager.WorldToGridPosition(slot.transform.position);
            Debug.Log("================Starting Merge Check================");

            _mergeableItemGroups.Clear();
            var currentCell = _gridManager.GetCell(gridPosition);

            if (currentCell.Tray == null)
            {
                Debug.Log($"No tray found at position {gridPosition}");
                return;
            }

            ResetPreviousMergePriorities();
            ScanNeighborsForMerge(currentCell, gridPosition);

            Debug.Log("Merge scan completed");
            _table.StartCoroutine(ProcessMergeGroups());
        }

        private IEnumerator ProcessMergeGroups()
        {
            yield return new WaitForSeconds(0.2f);
            ExecuteMergeGroups();
        }

        private void ScanNeighborsForMerge(Cell originCell, Vector2Int position)
        {
            foreach (var itemId in originCell.Tray.GetUniqueItemIDs())
            {
                InitializeMergeGroupIfNeeded(itemId);
                ScanAdjacentCells(position, itemId);
                AddTrayToMergeGroup(originCell.Tray, itemId, isOriginTray: true);
                UpdateMergeGroupPriorities(itemId);
            }
        }

        private void ScanAdjacentCells(Vector2Int centerPosition, string itemId)
        {
            foreach (var direction in directions)
            {
                var neighborPosition = new Vector2Int(direction.x + centerPosition.x, direction.y + centerPosition.y);
                if (IsValidNeighborCell(neighborPosition, out var neighborTray) && 
                    neighborTray.GetCountOfItem(itemId) > 0)
                {
                    AddTrayToMergeGroup(neighborTray, itemId);
                }
            }
        }

        private bool IsValidNeighborCell(Vector2Int position, out Tray tray)
        {
            tray = null;
            if (_gridManager.IsValidGridPosition(position) &&
                _gridManager.TryGetCell(position, out var cell) && 
                cell.HasTray)
            {
                tray = cell.Tray;
                return true;
            }
            return false;
        }

        private void ResetPreviousMergePriorities()
        {
            foreach (var candidate in _lastProcessedCandidates)
            {
                candidate.isOriginTray = false;
                candidate.RecalculatePriority();
            }
            _lastProcessedCandidates.Clear();
        }

        private void InitializeMergeGroupIfNeeded(string itemId)
        {
            if (!_mergeableItemGroups.ContainsKey(itemId))
            {
                _mergeableItemGroups[itemId] = new List<TrayMergeCandidate>();
                Debug.Log($"Initialized merge group for item {itemId}");
            }
        }

        private void AddTrayToMergeGroup(Tray tray, string itemId, bool isOriginTray = false)
        {
            var mergeCandidate = new TrayMergeCandidate();
            mergeCandidate.Initialize(tray, itemId, isOriginTray);
            _mergeableItemGroups[itemId].Add(mergeCandidate);

            if (isOriginTray)
            {
                _lastProcessedCandidates.Add(mergeCandidate);
            }
        }

        private void UpdateMergeGroupPriorities(string itemId)
        {
            if (_mergeableItemGroups.TryGetValue(itemId, out var candidates))
            {
                foreach (var candidate in candidates)
                {
                    candidate.RecalculatePriority();
                }
                candidates.Sort();
            }
        }

        private void ExecuteMergeGroups()
        {
            _table.StartCoroutine(ExecuteMergeGroupsSequentially());
        }

        private IEnumerator ExecuteMergeGroupsSequentially()
        {
            foreach (var group in _mergeableItemGroups)
            {
                Debug.Log($"Processing merge group: {group.Key}");
                MergeTrays(group.Value);
                yield return new WaitForSeconds(0.1f);
                RecalculateAllGroupPriorities();
            }

            yield return new WaitForSeconds(.5f);
            AnimateTrayClear();
        }

        private void RecalculateAllGroupPriorities()
        {
            foreach (var group in _mergeableItemGroups)
            {
                UpdateMergeGroupPriorities(group.Key);
            }
        }

        private void AnimateTrayClear()
        {
            foreach (var cell in _gridManager.TableMap)
            {
                cell.Value.Slot.PlayClearAnimation();
            }
        }

        private void MergeTrays(List<TrayMergeCandidate> candidates)
        {
            if (candidates.Count < 2)
            {
                Debug.Log("Insufficient trays for merging");
                return;
            }

            string targetItemId = candidates[0].ItemId;
            Queue<Tray> trayQueue = new Queue<Tray>(candidates.Select(c => c.Tray));
            Tray destinationTray = trayQueue.Dequeue();

            while (trayQueue.Count > 0)
            {
                Tray sourceTray = trayQueue.Peek();

                if (!destinationTray.CanAddMoreItem())
                {
                    destinationTray = trayQueue.Dequeue();
                    continue;
                }

                if (sourceTray.GetCountOfItem(targetItemId) == 0)
                {
                    trayQueue.Dequeue();
                    continue;
                }

                var itemToTransfer = sourceTray.GetFirstOfItem(targetItemId);
                if (itemToTransfer != null)
                {
                    sourceTray.Remove(itemToTransfer);
                    destinationTray.Add(itemToTransfer);

                    if (sourceTray.GetCountOfItem(targetItemId) == 0)
                    {
                        trayQueue.Dequeue();
                    }
                }
            }
        }
    }
}