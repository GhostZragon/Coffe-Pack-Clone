using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BaseView
{
    public Action OnResumeClicked;
    public Action OnBackMainMenuClicked;
    public Action OnResetButtonClicked;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button resetBtn;
    [SerializeField] private Button backMenuBtn;
   
    protected override void Register()
    {
        base.Register();
        resumeBtn.onClick.AddListener(OnResume);
        backMenuBtn.onClick.AddListener(OnBackMenu);
        resetBtn.onClick.AddListener(OnReset);
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        resumeBtn.onClick.RemoveListener(OnResume);
        backMenuBtn.onClick.RemoveListener(OnBackMenu);
        resetBtn.onClick.RemoveListener(OnReset);
    }

    private void OnResume()
    {
        OnResumeClicked?.Invoke();
    }

    private void OnBackMenu()
    {
        OnBackMainMenuClicked?.Invoke();
    }

    private void OnReset()
    {
        OnResetButtonClicked?.Invoke();
    }
}
