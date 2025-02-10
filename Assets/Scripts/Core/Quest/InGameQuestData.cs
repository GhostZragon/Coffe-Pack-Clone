using System;
using Sirenix.OdinInspector;
using UnityEngine;




[Serializable]
public class InGameQuestData
{
    [SerializeField,InfoBox("Game will fully random this quest if is random is true")]
    public bool isRandom = false;
    [HideIf(nameof(isRandom))]
    public string ItemID;
    [HideIf(nameof(isRandom))]
    public int TargetQuantity;

    public Sprite questIcon;
    
    public Action<int> OnUpdateItemCount;
    public Action OnCompleteQuest;
    public Action OnDestroyQuest;

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

    public void DestroyQuestUI()
    {
        OnDestroyQuest?.Invoke();
    }
    
    [Button]
    public void UpdateQuest(bool IsCompleteAll)
    {
        int quantity = IsCompleteAll ? TargetQuantity : 1;
        TargetQuantity -= quantity;
        
        RefreshUI();

        CompleteQuest();
    }

    public void InitByQuestData(QuestData questData)
    {
        ItemID = questData.ItemID;
        TargetQuantity = questData.TargetQuantity;
        questIcon = questData.questIcon;
    }

    public bool CanUpdateQuest(string itemID)
    {
        return itemID == ItemID && TargetQuantity > 0;
    }
}