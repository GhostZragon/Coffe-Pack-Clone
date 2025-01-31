using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle Quest Data",menuName = "SO/Puzzle Quest Data")]
public class PuzzleQuestData : ScriptableObject
{
    public List<PuzzleQuest> puzzleQuestList;
    [ShowIf("IsRandom")]
    public string ItemID;

    public bool IsRandom;
}