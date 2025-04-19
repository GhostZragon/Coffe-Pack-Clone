using System;
using UnityEngine;

[CreateAssetMenu(menuName = "DataController/GameDataController", fileName = "GameDataController")]
public class GameDataController : LocalDataController<GameData>
{
    public event SpecificValueChangedHandler OnCurrencyChanged;

    public string GetDisplayName() => _data.DisplayName;
    public int GetLives() => _data.Lives;
    public int GetCurrency() => _data.Currency;


    public void AddCurrency(int Amount)
    {
        SetCurrency(_data.Currency + Amount);
    }

    public void SetCurrency(int newAmount)
    {
        _data.Currency = newAmount;
        OnCurrencyChanged?.Invoke(_data.Currency);
    }

    public void SetDisplayName(string newDisplayName)
    {
        _data.DisplayName = newDisplayName;
    }

    public void SetLives(int lives)
    {
        _data.Lives = lives;
    }
}