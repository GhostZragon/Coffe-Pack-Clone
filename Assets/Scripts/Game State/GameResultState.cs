using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using UnityEngine;

public class GameResultState : BaseState
{
    private ResultUI resultUI;
  

    protected override void Register()
    {
        base.Register();
        resultUI = UIManager.Instance.gameplayUI.ShowResultMenu();

        resultUI.OnReplayClicked += OnReplayClicked;
        resultUI.OnBackMenuClicked += OnBackMenuClicked;
        PlayIncreaseCoinEffect();
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        resultUI.OnReplayClicked -= OnReplayClicked;
        resultUI.OnBackMenuClicked -= OnBackMenuClicked;
    }

    private void PlayIncreaseCoinEffect()
    {
        var gameSessionController = DataManager.Instance.GetDataController<GameSessionController>();

        resultUI.UpdateRewardCoinText(gameSessionController.Data.CoinReward);
    }

    private void CompleteEffectImmediate()
    {
        // add in future
        resultUI.motionHandles.Complete();
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