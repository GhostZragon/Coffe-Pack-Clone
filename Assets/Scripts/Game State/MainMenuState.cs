using System;
using Object = UnityEngine.Object;

public class MainMenuState : BaseState
{
    
    private LevelSelection levelSelection;
    private LevelPanelUI levelPanelUI;
    public override void PrepareState()
    {
        base.PrepareState();
        
        UIManager.Instance.ShowMenuUI();
        // init level map
        levelSelection.SettingsLevel();
        levelPanelUI.Initialize(levelSelection.MaxLevel);
        // Invoke event
        UIManager.Instance.menuUI.OnSelectLevel?.Invoke(levelSelection.CurrentLevel);
    }

    protected override void CatchRef()
    {
        base.CatchRef();
        levelSelection = Object.FindFirstObjectByType<LevelSelection>();
        levelPanelUI = UIManager.Instance.menuUI.LevelPanelUI;
        
    }

    protected override void Register()
    {
        base.Register();
        levelPanelUI.levelUnlockChecking = IsLevelUnlock;
     
        UIManager.Instance.menuUI.OnSelectLevel += OnSelectLevelClicked;
        UIManager.Instance.menuUI.OnPlayButtonClicked += OnPlayButtonClicked;
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        UIManager.Instance.menuUI.OnSelectLevel -= OnSelectLevelClicked;
        UIManager.Instance.menuUI.OnPlayButtonClicked -= OnPlayButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        // go to gameplay state
        ChangeState(new GameplayState());
    }

    private void OnSelectLevelClicked(int level)
    {
        // game logic when select level
        levelSelection.SetLevel(level);
        levelPanelUI.SelectLevelUI(level);

        UIManager.Instance.menuUI.CurrentLevelText.OnSelectLevel(level);
    }

    private bool IsLevelUnlock(int checkedLevel)
    {
        return levelSelection.IsLevelUnlock(checkedLevel);
    }


}