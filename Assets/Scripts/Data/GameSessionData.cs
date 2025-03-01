using System;

[Serializable]
public class GameSessionData
{
    private int starUnlocked;
    private int coinReward;
    private GameResult gameResult;

    public int StarUnlocked { get => starUnlocked; set => starUnlocked = value; }
    public int CoinReward { get => coinReward; set => coinReward = value; }
    public GameResult GameResult { get => gameResult; set => gameResult = value; }
}
