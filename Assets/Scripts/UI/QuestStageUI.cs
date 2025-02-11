using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class QuestStageUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private LevelStarUI levelStarUI;

    public static event Action<Vector3> OnStarChanged;
    
    private int maxStage = 0;

    private void Awake()
    {
        slider.value = 0;
    }

    public void SetMaxStage(int maxStage)
    {
        this.maxStage = maxStage;
        slider.maxValue = this.maxStage;
    }
    public void OnStageChanged(int stageChanged)
    {
        TweenSlider(stageChanged);
        
        OnStarChanged?.Invoke(levelStarUI.GetStarByIndex(stageChanged));
    }

    public Vector3 GetStarPosition()
    {
        return levelStarUI.GetStarByIndex(0);
    }
    
    private void TweenSlider(int newStage)
    {
        LMotion.Create(slider.value, newStage, 0.3f)
            .WithOnComplete(() =>
            {
                levelStarUI.ActiveStageUnlock(newStage);
            })
            .Bind((x) =>
        {
            slider.value = x;
        });
    }

    public void ResetUI()
    {
        
    }

}