using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseView
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private bool isTestingChangeScreen = false;
    [SerializeField] private Button playButton;

    public CurrentLevelText CurrentLevelText;
    public LevelPanelUI LevelPanelUI;
    public Action OnPlayButtonClicked;
    public Action<int> OnSelectLevel;

    protected override void Register()
    {
        base.Register();
        playButton.onClick.AddListener(PlayClicked);
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        playButton.onClick.RemoveListener(PlayClicked);
    }

    private void PlayClicked()
    {
        OnPlayButtonClicked?.Invoke();
    }

    private void Update()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }
}
