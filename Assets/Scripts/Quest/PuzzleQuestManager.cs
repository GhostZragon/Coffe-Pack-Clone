using System.Collections.Generic;
using UnityEngine;

public class PuzzleQuestManager : MonoBehaviour
{
    public static PuzzleQuestManager Instance;
    public PuzzleQuestManagerUI PuzzleQuestManagerUI;
    public List<PuzzleQuest> puzzleQuests;

    public List<string> randomItemsList = new();
    public int questCount = 3;
    public int stageCount = 3;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupQuest();
    }

    private void SetupQuest()
    {
        puzzleQuests.Clear();
        for (int i = 0; i < questCount; i++)
        {
            var puzzleQuest = new PuzzleQuest();
            
            CreateRandomProperty(puzzleQuest);
            
            if (PuzzleQuestManagerUI == null) return;
            
            PuzzleQuestManagerUI.GetPuzzleQuestUI(puzzleQuest);
          
            puzzleQuests.Add(puzzleQuest);
        }
    }

    private void CreateRandomProperty(PuzzleQuest puzzleQuest)
    {
        puzzleQuest.ItemID = GetRandomItemID();
        puzzleQuest.TargetQuantity = GetRandomItemCount();
    }

    private int GetRandomItemCount()
    {
        return Random.Range(1, 3);
    }

    private string GetRandomItemID()
    {
        return randomItemsList[Random.Range(0,randomItemsList.Count)];
    }
    
    public void OnCompleteItem(string itemID)
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            Debug.LogWarning("Item ID is null");
            return;
        }
        
        foreach (var quest in puzzleQuests)
        {
            if (quest.ItemID == itemID && quest.IsComplete == false)
            {
                quest.UpdateQuest();
                break;
            }

            quest.RefreshUI();
        }

        CheckCompleteQuestStage();
    }
    
    
    private void CheckCompleteQuestStage()
    {
        bool isCompleteAllQuest = true;
        foreach (var quest in puzzleQuests)
        {
            if (quest.IsComplete == false)
            {
                isCompleteAllQuest = false;
                break;
            }
        }

        if (isCompleteAllQuest == false) return;

        if (stageCount == 0)
        {
            // win Game
        }
        else
        {
            stageCount--;
            SetupQuest();
        }
    }
}