using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSessionController", menuName = "DataController/Game Session Controller")]
public class GameSessionController: RuntimeDataController<GameSessionData>
{
    public event SpecificValueChangedHandler OnMaxStageChange;
    public event SpecificValueChangedHandler OnCurrentStageChange;
    public event SpecificValueChangedHandler OnRewardCoinChange;

    public override Task LoadData()
    {
        InitData();
        return Task.CompletedTask;
    }

    public void ResetSession()
    {
        InitData();
    }

    public override void InitData()
    {
        _data = new GameSessionData();
        _data.StarUnlocked = 0;
        _data.CurrentStage = 0;
        _data.MaxStage = 0;
        _data.CoinReward = 0;
    }

    public void SetCurrentStage(int newCurrentStage)
    {
        Debug.Log("Set Current Stage: " + newCurrentStage);
        _data.CurrentStage = newCurrentStage;
        OnCurrentStageChange?.Invoke(_data.CurrentStage);
    }

    public void SetMaxStage(int newMaxStage)
    {
        Debug.Log("Set Max Stage: " + newMaxStage);
        _data.MaxStage = newMaxStage;
        OnMaxStageChange?.Invoke(_data.MaxStage);
    }
    
    public void AddRewardCoin(int amount)
    {
        _data.CoinReward += amount;
        OnRewardCoinChange?.Invoke(_data.CoinReward);
    }

    public void IncreaseCurrentStage()
    {
        SetCurrentStage(_data.CurrentStage += 1);
        _data.StarUnlocked += 1;
    }

    public int GetCurrentStage()
    {
        return _data.CurrentStage;
    }

    public void SetGameResult(GameResult gameResult)
    {
        _data.GameResult = gameResult;
    }

    public GameSessionData Data => _data;
}