using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle Quest Data",menuName = "SO/Puzzle Quest Data")]
public class PuzzleQuestData : ScriptableObject
{
    public QuestData[] stage1;
    public QuestData[] stage2;
    public QuestData[] stage3;

    public List<string> GetUniqueItemID()
    {
        HashSet<string> uniqueID = new();
        
        GetUniqueItemID(uniqueID, stage1);
        GetUniqueItemID(uniqueID, stage2);
        GetUniqueItemID(uniqueID, stage3);
        
        return new List<string>(uniqueID);
    }

    private void GetUniqueItemID(HashSet<string> hashSet,QuestData[] questDatas)
    {
        foreach (var questData in questDatas)
        {
            hashSet.Add(questData.ItemID);
        }
    }
    
}

public enum QuestType
{
    Predefine,
    Randomly
}
[Serializable]
public struct QuestData
{
    public QuestType Type;
    [HideIf(nameof(CanShow))]
    public string ItemID;
    [HideIf(nameof(CanShow))]
    public int TargetQuantity;
    
    private bool CanShow()
    {
        return Type == QuestType.Randomly;
    }

    public bool IsRandomly()
    {
        return Type == QuestType.Randomly;
    }
}