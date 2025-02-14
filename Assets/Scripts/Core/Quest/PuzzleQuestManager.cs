using System;
using System.Collections.Generic;
using System.Linq;
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
    [Header("UI")] [SerializeField] private PuzzleQuestData puzzleQuestData;
    [SerializeField] private int currentStage = 0;
    [SerializeField] private bool completeOneTime;
    [Header("Questing")] [SerializeField] private List<string> randomItemsList = new();
    [SerializeField] private List<InGameQuestData> inGameQuestDataList;
    [SerializeField] private PuzzleQuestEffectUI puzzleQuestEffectUI;
    private int maxStage = 0;

    private QuestFactory questFactory;

    private Dictionary<int, QuestData[]> questDataPerStage;

    public Action<int> OnChangedStage;

    private void Awake()
    {
        questFactory = new(randomItemsList);

        EventManager.Current._Game.OnCompleteItem += OnCompleteItem;
    }

    private void OnDestroy()
    {
        EventManager.Current._Game.OnCompleteItem -= OnCompleteItem;
    }

    public void SetPuzzleQuestData(PuzzleQuestData puzzleQuestData)
    {
        this.puzzleQuestData = puzzleQuestData;
        questDataPerStage = new Dictionary<int, QuestData[]>
        {
            [0] = puzzleQuestData.stage1,
            [1] = puzzleQuestData.stage2,
            [2] = puzzleQuestData.stage3
        };

        foreach (var item in questDataPerStage)
        {
            if (item.Value == null)
                continue;
            maxStage++;
        }
    }

    public void SetFirstState()
    {
        currentStage = 0;
    }

    public void CreateQuests()
    {
        CreateNewQuest();
    }

    private void OnCompleteItem(ItemInfo itemInfo)
    {
        var quest = inGameQuestDataList.FirstOrDefault(q => q.CanUpdateQuest(itemInfo.ItemId));
        if (quest != null)
        {
            quest.UpdateQuest(completeOneTime);
            puzzleQuestEffectUI.CreateEffectToStar(itemInfo.WorldPosition);
            Debug.Log("Check complete item: " + itemInfo.ItemId);
        }
        else
        {
            puzzleQuestEffectUI.CreateEffectToCollector(itemInfo.WorldPosition);
        }

        if (IsFinishAllQuestCurrentStage())
        {
            GoNextStage();
        }
    }


    [Button]
    private void GoNextStage()
    {
        currentStage += 1;

        OnChangedStage?.Invoke(currentStage);

        CreateNewQuest();
    }

    private bool IsFinishAllQuestCurrentStage()
    {
        return inGameQuestDataList.All(quest => quest.IsComplete);
    }

    public bool IsRunOutOfQuest()
    {
        return IsFinishAllQuestCurrentStage() && questDataPerStage.ContainsKey(currentStage + 1);
    }

    private bool IsContainQuestDataForCurrentState(int puzzleStage, out QuestData[] arrayQuest)
    {
        return questDataPerStage.TryGetValue(puzzleStage, out arrayQuest);
    }

    public void ClearQuest()
    {
        foreach (var quest in inGameQuestDataList)
        {
            quest.DestroyQuestUI();
        }

        inGameQuestDataList.Clear();
    }

    private void CreateNewQuest()
    {
        if (!IsContainQuestDataForCurrentState(currentStage, out var arrayQuest)) return;

        foreach (var item in inGameQuestDataList)
        {
            item.DestroyQuestUI();
        }
        inGameQuestDataList.Clear();
        // split init and update logic 
        for (int i = 0; i < arrayQuest.Length; i++)
        {
            var inGameQuestData = questFactory.CreateQuest(arrayQuest[i]);
            inGameQuestDataList.Add(inGameQuestData);
            
            EventManager.Current._UI.OnBindingWithQuestUI?.Invoke(inGameQuestData);
            Debug.Log($"Create quest {inGameQuestData.ItemID} and {inGameQuestData.TargetQuantity}");
        }
    }

    public int GetMaxStage()
    {
        return maxStage;
    }
}