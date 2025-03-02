using System;
using System.Collections.Generic;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class QuestStageUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private LevelStarProgressUI levelStarProgressUI;
    
    [Header("Stage Configuration")]
    [SerializeField] private int currentStage = 0;
    [SerializeField] private int maxStage = 0;

    private Dictionary<int, float> stageProgressValues;
    private GameSessionController gameSessionController;


    public int GetCurrentStageUI()
    {
        return currentStage;
    }

    private void Start()
    {
        Initialize();

    }

    private void Initialize()
    {
        progressSlider.value = 0;
        stageProgressValues = new();

        gameSessionController = DataManager.Instance.GetDataController<GameSessionController>();

        gameSessionController.OnMaxStageChange += SetMaxStage;
        gameSessionController.OnCurrentStageChange += HandleStageChanged;
    }

    private void OnDestroy()
    {
        gameSessionController.OnMaxStageChange -= SetMaxStage;
        gameSessionController.OnCurrentStageChange -= HandleStageChanged;
    }

    [Button]
    public void SetMaxStage(int maxStage)
    {
        if (maxStage < 0)
        {
            Debug.LogWarning($"Invalid maxStage value: {maxStage}", this);
            return;
        }

        Debug.Log("Set max stage: " + maxStage, gameObject);

        stageProgressValues.Clear();

        this.maxStage = maxStage;

        progressSlider.maxValue = this.maxStage;

        levelStarProgressUI.SetMaxStar(this.maxStage);

        CalculateStageProgressValues(maxStage);
    }

    private void CalculateStageProgressValues(int maxStage)
    {
        // Initialize dictionary with expected capacity
        stageProgressValues = new Dictionary<int, float>(maxStage + 1);

        // Stage 0 always represents 0 progress
        stageProgressValues[0] = 0;

        // Calculate normalized progress value for each stage
        for (int stage = 1; stage <= maxStage; stage++)
        {
            stageProgressValues[stage] = (float)stage / this.maxStage;
        }
    }

    public void HandleStageChanged(int newStage)
    {
        if (newStage > maxStage)
        {
            Debug.LogWarning($"Stage value {newStage} exceeds max stage {maxStage}", this);
            return;
        }

        Debug.Log($"Changed stage: {newStage}", gameObject);
        currentStage = newStage;
        AnimateProgressBar();
    }

    private void AnimateProgressBar()
    {
        if (progressSlider == null || levelStarProgressUI == null || !stageProgressValues.ContainsKey(currentStage))
        {
            Debug.LogWarning("Cannot animate progress bar - missing components or invalid stage", this);
            return;
        }

        float targetValue = stageProgressValues[currentStage];
        levelStarProgressUI.ActiveStageUnlock(currentStage);

        // Animate the slider to the new value
        LMotion.Create(progressSlider.value, targetValue, 1f)
               .WithEase(Ease.OutQuad)
               .Bind(value => progressSlider.value = value);
    }

    /// <summary>
    /// Resets the progress UI to the initial state
    /// </summary>
    [Button("Reset Progress")]
    public void ResetProgressUI()
    {
        currentStage = 0;

        if (progressSlider != null)
        {
            progressSlider.value = 0;
        }

        if (levelStarProgressUI != null)
        {
            levelStarProgressUI.ActiveStageUnlock(0);
        }
    }
}