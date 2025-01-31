using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum PuzzleStage
{
    First,
    Second,
    Third
}

public class PuzzleQuestManager : MonoBehaviour
{
    public static PuzzleQuestManager Instance;
    [SerializeField] private PuzzleQuestManagerUI PuzzleQuestManagerUI;
    [SerializeField] private List<PuzzleQuest> puzzleQuests;

    [SerializeField] private List<string> randomItemsList = new();
    [SerializeField] private int stageCount = 3;
    [SerializeField] private PuzzleQuestData puzzleQuestData;
    private Dictionary<PuzzleStage, QuestData[]> questDataPerStage;
    [SerializeField] private PuzzleStage currentStage = PuzzleStage.First;

    [SerializeField] private bool completeOneTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        questDataPerStage = new()
        {
            [PuzzleStage.First] = puzzleQuestData.stage1,
            [PuzzleStage.Second] = puzzleQuestData.stage2,
            [PuzzleStage.Third] = puzzleQuestData.stage3
        };

        CreateNewQuest();
    }

    [Button]
    private void CompleteAll()
    {
        PuzzleStage previousStage = currentStage;
        if (questDataPerStage.TryGetValue(currentStage, out var arrayQuest))
        {
            foreach (var item in arrayQuest)
            {
                for (int i = 0; i < item.TargetQuantity; i++)
                {
                    OnCompleteItem(item.ItemID);

                    if (previousStage != currentStage)
                        break;
                }
            }
        }
    }

    private void CreateNewQuest()
    {
        puzzleQuests.Clear();
        if (CanGoNextStage(currentStage, out var arrayQuest))
        {
            foreach (var questData in arrayQuest)
            {
                var puzzleQuest = new PuzzleQuest();
                if (questData.IsRandomly())
                {
                    CreateRandomProperty(puzzleQuest);
                }
                else
                {
                    puzzleQuest.InitByQuestData(questData);
                }

                if (PuzzleQuestManagerUI == null) return;
                Debug.Log($"Create quest {puzzleQuest.ItemID} and {puzzleQuest.TargetQuantity}");
                PuzzleQuestManagerUI.GetPuzzleQuestUI(puzzleQuest);

                puzzleQuests.Add(puzzleQuest);
            }
        }
        else
        {
            Win();
        }
    }

    private void CreateRandomProperty(PuzzleQuest puzzleQuest)
    {
        puzzleQuest.ItemID = GetRandomItemID();
        puzzleQuest.TargetQuantity = GetRandomItemCount();
        puzzleQuest.isRandom = true;
    }

    private int GetRandomItemCount()
    {
        return Random.Range(1, 3);
    }

    private string GetRandomItemID()
    {
        return randomItemsList[Random.Range(0, randomItemsList.Count)];
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
                Debug.Log("Check complete item: " + itemID);
                int quantity = completeOneTime ? quest.TargetQuantity : 1;
                quest.UpdateQuest(quantity);
                break;
            }

            quest.RefreshUI();
        }

        CheckCompleteStage();
    }
    

    [Button]
    private void CheckCompleteStage()
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

        CheckWin();
    }

    private void CheckWin()
    {
        currentStage = currentStage == PuzzleStage.First ? PuzzleStage.Second : PuzzleStage.Third;

        CreateNewQuest();
    }

    private bool CanGoNextStage(PuzzleStage puzzleStage, out QuestData[] arrayQuest)
    {
        return questDataPerStage.TryGetValue(puzzleStage, out arrayQuest);
    }

    private void Win()
    {
        Debug.Log("Win Game");
    }

    public List<string> GetItemTypeInQuest()
    {
        List<string> itemList = new();

        foreach (var quest in puzzleQuests)
        {
            if (itemList.Contains(quest.ItemID))
                continue;
            itemList.Add(quest.ItemID);
        }
        
        return itemList;
    }
}