using System;
using System.Collections.Generic;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class QuestStageUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField ]private int currentStage = 0;

    private int maxStage = 0;

    private Dictionary<int, float> values;

    public LevelStarProgressUI levelStarUI;
    public int CurrentStage
    {
        get => currentStage;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        slider.value = 0;
        values = new();
        values.Add(0,0);
        values.Add(1,1.5f);
        values.Add(2,3f);
    }

    public void SetMaxStage(int maxStage)
    {
        this.maxStage = maxStage;
        slider.maxValue = this.maxStage;
       
    }

    public void OnStageChanged(int stageChanged)
    {
        currentStage = stageChanged;
        TweenSliderByCurrentLevel();
    }
    [Button]
    private void TweenSliderByCurrentLevel()
    {
        var sliderValue = values[currentStage];
        levelStarUI.ActiveStageUnlock(currentStage);
        LMotion.Create(slider.value, sliderValue, 1)
            .Bind((x) => { slider.value = x; });
    }
    
    [Button]
    public void ResetUI()
    {
        slider.value = 0;
        levelStarUI.ActiveStageUnlock(0);
    }
}