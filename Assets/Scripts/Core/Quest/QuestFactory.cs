using System.Collections.Generic;
using UnityEngine;

public class QuestFactory
{
    private List<string> randomItemsList;

    public QuestFactory(List<string> randomItems)
    {
        randomItemsList = randomItems;
    }

    public InGameQuestData CreateQuest(QuestData questData)
    {
        return UpdateOrCreateInGameQuestData(questData);
    }

    private InGameQuestData UpdateOrCreateInGameQuestData(QuestData questData)
    {
        InGameQuestData inGameQuestData = new();
       
        if (questData.IsRandomly())
        {
            AssignRandomProperties(inGameQuestData);
        }
        else
        {
            inGameQuestData.InitByQuestData(questData);
        }

        return inGameQuestData;
    }
    
    private void AssignRandomProperties(InGameQuestData quest)
    {
        quest.ItemID = GetRandomItemID();
        quest.TargetQuantity = GetRandomItemCount();
        quest.isRandom = true;
    }

    private int GetRandomItemCount()
    {
        return Random.Range(1, 3);
    }

    private string GetRandomItemID()
    {
        return randomItemsList[Random.Range(0, randomItemsList.Count)];
    }
}