using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BaseView
{
    public Action OnResumeClicked;
    public Action OnBackMainMenuClicked;

    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button backMenuBtn;
   
    protected override void Register()
    {
        base.Register();
        resumeBtn.onClick.AddListener(OnResume);
        backMenuBtn.onClick.AddListener(OnBackMenu);
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        resumeBtn.onClick.RemoveListener(OnResume);
        backMenuBtn.onClick.RemoveListener(OnBackMenu);
    }

    private void OnResume()
    {
        OnResumeClicked?.Invoke();
    }

    private void OnBackMenu()
    {
        OnBackMainMenuClicked?.Invoke();
    }
}
