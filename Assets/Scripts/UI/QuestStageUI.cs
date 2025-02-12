using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class QuestStageUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public LevelStarProgressUI levelStarUI;


    private int maxStage = 0;
    private int currentStage = 0;

    public int CurrentStage
    {
        get => currentStage;
    }

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
        currentStage = stageChanged;
        TweenSlider(stageChanged);
    }


    private void TweenSlider(int newStage)
    {
        LMotion.Create(slider.value, newStage, 0.3f)
            .WithOnComplete(() => { levelStarUI.ActiveStageUnlock(newStage); })
            .Bind((x) => { slider.value = x; });
    }

    public void ResetUI()
    {
        slider.value = 0;
        levelStarUI.ActiveStageUnlock(0);
    }
}