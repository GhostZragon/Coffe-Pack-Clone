using System;
using System.Collections.Generic;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class QuestStageUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private int currentStageUI = 0;

    private int maxStage = 0;

    private Dictionary<int, float> values;

    public LevelStarProgressUI levelStarUI;

    public int GetCurrentStageUI()
    {
        return currentStageUI;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        slider.value = 0;
        values = new();

        PuzzleQuestManager.OnSetMaxStage += SetMaxStage;
    }

    private void OnDestroy()
    {
        PuzzleQuestManager.OnSetMaxStage -= SetMaxStage;
    }

    [Button]
    public void SetMaxStage(int maxStage)
    {
        Debug.Log("Set max stage: "+maxStage,gameObject);
        
        values.Clear();
        
        this.maxStage = maxStage;
        
        slider.maxValue = this.maxStage;
        
        levelStarUI.SetMaxStar(this.maxStage);
     
        InitTweenValue(maxStage);
    }

    private void InitTweenValue(int maxStage)
    {
        values[0] = 0;
        for (int i = 1; i <= maxStage; i++)
        {
            values[i] = (float)i / this.maxStage ;
        }
    }

    public void OnStageChanged(int stageChanged)
    {
        if (stageChanged > maxStage) return;
        Debug.Log("Changed stage: "+stageChanged,gameObject);

        currentStageUI = stageChanged;
        TweenSliderByCurrentLevel();
    }
    [Button]
    private void TweenSliderByCurrentLevel()
    {
        var sliderValue = values[currentStageUI];
        
        levelStarUI.ActiveStageUnlock(currentStageUI);
      
        LMotion.Create(slider.value, sliderValue, 1)
            .Bind((x) => { slider.value = x; });
    }
    
    [Button]
    public void ResetProgressUI()
    {
        slider.value = 0;
        levelStarUI.ActiveStageUnlock(0);
    }
}