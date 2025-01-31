using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleQuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Image icon;
    [SerializeField] private Image iconShadow;

    public void BindingUI(PuzzleQuest puzzleQuest)
    {
        itemNameText.text = $"ID: {puzzleQuest.ItemID}";
        UpdateCount(puzzleQuest.TargetQuantity);

        puzzleQuest.OnUpdateItemCount = UpdateCount;
        puzzleQuest.OnCompleteQuest = OnCompleteQuest;
    }

    private void UpdateCount(int targetQuantity)
    {
        itemCountText.text = "x" + targetQuantity;
    }

    private void OnCompleteQuest()
    {
        Destroy(gameObject);
    }
}