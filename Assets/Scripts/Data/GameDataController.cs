using System;
using UnityEngine;

[CreateAssetMenu(menuName = "DataController/GameDataController", fileName = "GameDataController")]
public class GameDataController : LocalDataController<GameData>
{
    public string GetDisplayName() => _data.DisplayName;
    public int GetLives() => _data.Lives;
    public int GetCurrency() => _data.Currency;

    public void SetDisplayName(string newDisplayName)
    {
        _data.DisplayName = newDisplayName;
    }

    public void SetLives(int lives)
    {
        _data.Lives = lives;
    }
}