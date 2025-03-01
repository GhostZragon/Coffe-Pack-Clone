public readonly struct ResultData
{
    public int StarUnlocked { get; }
    public int CoinReward { get; }
    public GameResult Result { get; }

    public ResultData(int star, int coin, GameResult _result)
    {
        StarUnlocked = star;
        CoinReward = coin;
        Result = _result;
    }
}
