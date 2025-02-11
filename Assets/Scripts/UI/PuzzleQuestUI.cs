using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleQuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Image icon;

    public void BindingUI(InGameQuestData inGameQuestData)
    {
        itemNameText.text = $"ID: {inGameQuestData.ItemID}";
        UpdateCount(inGameQuestData.TargetQuantity);

        inGameQuestData.OnUpdateItemCount = UpdateCount;
        inGameQuestData.OnCompleteQuest = OnCompleteQuest;
        inGameQuestData.OnDestroyQuest = OnDestroyQuest;

        if (inGameQuestData.questIcon == null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    private void UpdateCount(int targetQuantity)
    {
        itemCountText.text = "x" + targetQuantity;
    }

    private void OnCompleteQuest()
    {
        Destroy(gameObject);
    }

    private void OnDestroyQuest()
    {
        Destroy(gameObject);
    }
}