public class GameResultState : BaseState
{
    private ResultData resultData;
    private ResultUI resultUI;
    public GameResultState(ResultData resultData)
    {
        this.resultData = resultData;
    }


    protected override void AfterPrepareState()
    {
        base.AfterPrepareState();
        resultUI = UIManager.Instance.gameplayUI.ShowResultMenu(resultData);

    }

    protected override void Register()
    {
        base.Register();
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