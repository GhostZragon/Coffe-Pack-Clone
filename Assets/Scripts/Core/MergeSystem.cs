using System.Collections.Generic;
using UnityEngine;

public partial class Table
{
    public class MergeSystem
    {
        public void Merge(List<PriorityTray> sources)
        {
            if (sources.Count < 2)
            {
                Debug.Log("Danh sách tray này không đủ để merge");
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
    }
}