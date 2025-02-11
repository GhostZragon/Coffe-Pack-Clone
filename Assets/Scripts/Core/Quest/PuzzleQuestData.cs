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

    public Sprite questIcon;

    private bool CanShow()
    {
        return Type == QuestType.Randomly;
    }

    public bool IsRandomly()
    {
        return Type == QuestType.Randomly;
    }
}