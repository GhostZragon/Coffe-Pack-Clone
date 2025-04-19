using System;
using UnityEngine;
[Serializable]
public class GameSessionData
{
    [SerializeField] private int starUnlocked;
    [SerializeField] private int coinReward;
    [SerializeField] private GameResult gameResult;
    [SerializeField] private int maxStage;
    [SerializeField] private int currentStage;
    public int StarUnlocked { get => starUnlocked; set => starUnlocked = value; }
    public int CoinReward { get => coinReward; set => coinReward = value; }
    public GameResult GameResult { get => gameResult; set => gameResult = value; }
    public int MaxStage { get => maxStage; set => maxStage = value; }
    public int CurrentStage { get => currentStage; set => currentStage = value; }
}
