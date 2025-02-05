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
    [Header("UI")]
   
    [SerializeField] private PuzzleQuestData puzzleQuestData;
    [SerializeField] private PuzzleStage currentStage = PuzzleStage.First;
    [SerializeField] private bool completeOneTime;
    [Header("Questing")]
    [SerializeField] private List<string> randomItemsList = new();
    [SerializeField] private List<InGameQuestData> InGameQuestDataList;
    
    
    private Dictionary<PuzzleStage, QuestData[]> questDataPerStage;

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

    public void SetPuzzleQuestData(PuzzleQuestData puzzleQuestData)
    {
        this.puzzleQuestData = puzzleQuestData;
    }

    public void SetFirstState()
    {
        currentStage = PuzzleStage.First;
    }
    
    public void CreateQuests()
    {
        CreateNewQuest();
    }
    
    private void CreateNewQuest()
    {
        InGameQuestDataList.Clear();

        if (!IsContainQuestDataForCurrentState(currentStage, out var arrayQuest)) return;
        
        foreach (var questData in arrayQuest)
        {
            var puzzleQuest = CreatePuzzleQuest(questData);
            InGameQuestDataList.Add(puzzleQuest);

            Debug.Log($"Create quest {puzzleQuest.ItemID} and {puzzleQuest.TargetQuantity}");
            
            EventManager.Current._UI.OnBindingWithQuestUI?.Invoke(puzzleQuest);
          
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

    public void OnCompleteItem(string itemID)
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            Debug.LogWarning("Item ID is null");
            return;
        }

        foreach (var inGameQuestData in InGameQuestDataList)
        {
            if (!inGameQuestData.CanUpdateQuest(itemID)) continue;
            inGameQuestData.UpdateQuest(1);
            
            Debug.Log("Check complete item: " + itemID);
            break;
        }

        if(IsFinishAllQuest())
            return;
        GoNextStage();
    }



    [Button]
    private void GoNextStage()
    {
        currentStage = currentStage == PuzzleStage.First ? PuzzleStage.Second : PuzzleStage.Third;

        CreateNewQuest();
    }

    public bool IsFinishAllQuest()
    {
        foreach (var quest in InGameQuestDataList)
        {
            if (quest.IsComplete == false)
            {
                return false;
            }
        }
        return true && currentStage == PuzzleStage.Third;
    }

    private bool IsContainQuestDataForCurrentState(PuzzleStage puzzleStage, out QuestData[] arrayQuest)
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

        foreach (var quest in InGameQuestDataList)
        {
            if (itemList.Contains(quest.ItemID))
                continue;
            itemList.Add(quest.ItemID);
        }
        
        return itemList;
    }


    public void ClearQuest()
    {
        foreach (var quest in InGameQuestDataList)
        {
            quest.DestroyQuestUI();
        }
        InGameQuestDataList.Clear();
    }
    
}