using System;
using Sirenix.OdinInspector;
using UnityEngine;




[Serializable]
public class PuzzleQuest
{
    [SerializeField,InfoBox("Game will fully random this quest if is random is true")]
    public bool isRandom = false;
    [HideIf(nameof(isRandom))]
    public string ItemID;
    [HideIf(nameof(isRandom))]
    public int TargetQuantity;
   
    
    public Action<int> OnUpdateItemCount;
    public Action OnCompleteQuest;

    public bool IsComplete => TargetQuantity == 0;

    public void RefreshUI()
    {
        OnUpdateItemCount?.Invoke(TargetQuantity);
    }

    private void CompleteQuest()
    {
        if(TargetQuantity <= 0)
            OnCompleteQuest?.Invoke();
    }
    [Button]
    public void UpdateQuest()
    {
        --TargetQuantity;
    
        RefreshUI();

        CompleteQuest();
    }

    public void InitByQuestData(QuestData questData)
    {
        ItemID = questData.ItemID;
        TargetQuantity = questData.TargetQuantity;
    }
}