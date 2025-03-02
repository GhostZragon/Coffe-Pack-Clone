public class GameResultState : BaseState
{
    private ResultUI resultUI;
  

    protected override void Register()
    {
        base.Register();
        resultUI = UIManager.Instance.gameplayUI.ShowResultMenu();

        resultUI.OnReplayClicked += OnReplayClicked;
        resultUI.OnBackMenuClicked += OnBackMenuClicked;
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        resultUI.OnReplayClicked -= OnReplayClicked;
        resultUI.OnBackMenuClicked -= OnBackMenuClicked;
    }

    private void OnBackMenuClicked()
    {
        ChangeState(new MainMenuState());
    }
    
    private void OnReplayClicked()
    {
        ChangeState(new GameplayState());
    }

}