using TMPro;
using UnityEngine;

public class PuzzleQuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    
    public void BindingUI(PuzzleQuest puzzleQuest)
    {
        itemNameText.text = $"ID: {puzzleQuest.ItemID}";
        UpdateCount(puzzleQuest.TargetQuantity);
        
        puzzleQuest.OnUpdateItemCount = UpdateCount;
        puzzleQuest.OnCompleteQuest = OnCompleteQuest;
    }

    private void UpdateCount(int targetQuantity)
    {
        itemCountText.text = targetQuantity.ToString();
    }

    private void OnCompleteQuest()
    {
        Destroy(gameObject);
    }
}
