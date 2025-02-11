using System;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleQuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Image icon;
    [Header("Motion")]
    [SerializeField] private GameObject view;

    public void BindingUI(InGameQuestData inGameQuestData)
    {
        itemNameText.text = $"ID: {inGameQuestData.ItemID}";
        UpdateCount(inGameQuestData.TargetQuantity);

        if (inGameQuestData.questIcon == null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void UpdateCount(int targetQuantity)
    {
        itemCountText.text = "x" + targetQuantity;
    }

    public void OnInitEffect()
    {
        ScaleView(Vector3.zero * .8f, Vector3.one, 0.25f, Ease.InQuad);
    }
    
    public void OnCompleteQuest()
    {
        ScaleView(Vector3.one, Vector3.zero, 0.25f, Ease.OutQuad, () =>
        {
            Destroy(gameObject);
        });
    }

    public void OnDestroyQuest()
    {
        Destroy(gameObject);
    }

    private void ScaleView(Vector3 startScale,Vector3 endScale,float duration, Ease ease, Action OnComplete = null)
    {
        LMotion.Create(startScale, endScale, duration)
            .WithEase(ease)
            .WithOnComplete(OnComplete)
            .BindToLocalScale(view.transform);
    }
    
}