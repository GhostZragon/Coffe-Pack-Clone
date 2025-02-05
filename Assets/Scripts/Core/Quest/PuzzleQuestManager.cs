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
    [Header("UI")] 
    [SerializeField] private PuzzleQuestData puzzleQuestData;
    [SerializeField, ReadOnly] private int currentStage = 0;
    [SerializeField] private bool completeOneTime;
    [Header("Questing")] 
    [SerializeField] private List<string> randomItemsList = new();
    [SerializeField] private List<InGameQuestData> inGameQuestDataList;


    private Dictionary<int, QuestData[]> questDataPerStage;

    private void Awake()
    {
        EventManager.Current._Game.OnCompleteItem += OnCompleteItem;
    }

    private void OnDestroy()
    {
        EventManager.Current._Game.OnCompleteItem -= OnCompleteItem;
    }

    [Button]
    private void CompleteAll()
    {
        var previousStage = currentStage;
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

    public void SetPuzzleQuestData(PuzzleQuestData puzzleQuestData)
    {
        this.puzzleQuestData = puzzleQuestData;
        questDataPerStage = new Dictionary<int, QuestData[]>
        {
            [0] = puzzleQuestData.stage1,
            [1] = puzzleQuestData.stage2,
            [2] = puzzleQuestData.stage3
        };
    }

    public void SetFirstState()
    {
        currentStage = 0;
    }

    public void CreateQuests()
    {
        CreateNewQuest();
    }

    private void CreateNewQuest()
    {
        if (!IsContainQuestDataForCurrentState(currentStage, out var arrayQuest)) return;

        for (int i = 0; i < arrayQuest.Length; i++)
        {
            if (i < inGameQuestDataList.Count)
            {
                // Cập nhật dữ liệu nhiệm vụ thay vì tạo mới
                inGameQuestDataList[i].InitByQuestData(arrayQuest[i]);
            }
            else
            {
                var puzzleQuest = CreatePuzzleQuest(arrayQuest[i]);
                inGameQuestDataList.Add(puzzleQuest);
                Debug.Log($"Create quest {puzzleQuest.ItemID} and {puzzleQuest.TargetQuantity}");
                EventManager.Current._UI.OnBindingWithQuestUI?.Invoke(puzzleQuest);
            }
        }
    }

    private InGameQuestData CreatePuzzleQuest(QuestData questData)
    {
        var inGameQuestData = new InGameQuestData();
        if (questData.IsRandomly())
        {
            CreateRandomProperty(inGameQuestData);
        }
        else
        {
            inGameQuestData.InitByQuestData(questData);
        }

        return inGameQuestData;
    }

    private void CreateRandomProperty(InGameQuestData inGameQuestData)
    {
        inGameQuestData.ItemID = GetRandomItemID();
        inGameQuestData.TargetQuantity = GetRandomItemCount();
        inGameQuestData.isRandom = true;
    }

    private int GetRandomItemCount()
    {
        return Random.Range(1, 3);
    }

    private string GetRandomItemID()
    {
        return randomItemsList[Random.Range(0, randomItemsList.Count)];
    }

    private void OnCompleteItem(string itemID)
    {
        var quest = inGameQuestDataList.FirstOrDefault(q => q.CanUpdateQuest(itemID));
        if (quest != null)
        {
            quest.UpdateQuest(completeOneTime);
            Debug.Log("Check complete item: " + itemID);
        }

        if (IsFinishAllQuestCurrentStage())
        {
            GoNextStage();
        }
    }


    [Button]
    private void GoNextStage()
    {
        currentStage +=1;

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

    public List<string> GetItemTypeInQuest()
    {
        List<string> itemList = new();

        foreach (var quest in inGameQuestDataList)
        {
            if (itemList.Contains(quest.ItemID))
                continue;
            itemList.Add(quest.ItemID);
        }

        return itemList;
    }


    public void ClearQuest()
    {
        foreach (var quest in inGameQuestDataList)
        {
            quest.DestroyQuestUI();
        }

        inGameQuestDataList.Clear();
    }
}