using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
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
    //[SerializeField] private int currentStage = 0;
    [SerializeField] private bool completeOneTime;

    [Header("Questing")] [SerializeField] private List<string> randomItemsList = new();
    [SerializeField] private List<InGameQuestData> inGameQuestDataList;
    [SerializeField] private PuzzleQuestEffectUI puzzleQuestEffectUI;

    //private int maxStage = 0;
    private QuestFactory questFactory;
    private Dictionary<int, QuestData[]> questDataPerStage;

    private GameSessionController gameSessionController;

    public Action<InGameQuestData> OnBindingQuestToUIAction;

    private void Awake()
    {
        questFactory = new(randomItemsList);
    }

    private void Start()
    {
        gameSessionController = DataManager.Instance.GetDataController<GameSessionController>();
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
        int maxStage = 0;
        foreach (var item in questDataPerStage)
        {
            if (item.Value == null)
                continue;
            maxStage++;
        }

        gameSessionController.SetMaxStage(maxStage);
    }

    public void SetFirstState()
    {
        gameSessionController.SetCurrentStage(0);
    }

    public void CreateQuests()
    {
        CreateNewQuest();
    }

    public void OnCompleteItem(ItemInfo itemInfo)
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
        Debug.Log("Is final stage of quest, you can win");
        gameSessionController.IncreaseCurrentStage();
        CreateNewQuest();
    }

    public bool IsFinishAllQuestCurrentStage()
    {
        return inGameQuestDataList.All(quest => quest.IsComplete);
    }

    public bool IsFinalStage()
    {
        return questDataPerStage.ContainsKey(gameSessionController.GetCurrentStage() + 1) == false;
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
        if (!IsContainQuestDataForCurrentState(gameSessionController.GetCurrentStage(), out var arrayQuest)) return;

        ClearQuest();

        // split init and update logic 
        for (int i = 0; i < arrayQuest.Length; i++)
        {
            var inGameQuestData = questFactory.CreateQuest(arrayQuest[i]);
            inGameQuestDataList.Add(inGameQuestData);

            OnBindingQuestToUIAction?.Invoke(inGameQuestData);
            Debug.Log($"Create quest {inGameQuestData.ItemID} and {inGameQuestData.TargetQuantity}");
        }
    }


    public bool IsItemOnQuesting(string itemID)
    {
        if (!IsContainQuestDataForCurrentState(gameSessionController.GetCurrentStage(), out var arrayQuest)) return false;

        foreach (var quest in arrayQuest)
        {
            if (quest.ItemID == itemID)
                return true;
        }

        return false;
    }
}