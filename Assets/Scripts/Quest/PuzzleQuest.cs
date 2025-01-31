using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class PuzzleQuest
{
    [ShowIf("IsRandom")]
    public string ItemID;
    [ShowIf("IsRandom")]
    public int TargetQuantity;
    public Action<int> OnUpdateItemCount;
    public Action OnCompleteQuest;

    public bool IsRandom = false;
    public bool IsComplete => TargetQuantity == 0;

    public void RefreshUI()
    {
        OnUpdateItemCount?.Invoke(TargetQuantity);
    }

    private void CompleteQuest()
    {
        if(IsComplete)
            OnCompleteQuest?.Invoke();
        OnCompleteQuest = null;
    }
    
    public void UpdateQuest()
    {
        TargetQuantity--;
        TargetQuantity = Mathf.Min(0, TargetQuantity);
    
        RefreshUI();

        CompleteQuest();
    }
    
}