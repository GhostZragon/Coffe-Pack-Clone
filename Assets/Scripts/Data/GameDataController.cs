public class GameDataController : LocalDataController<GameData>
{
    public string GetDisplayName() => _data.DisplayName;
    public int GetLives() => _data.Lives;
    public int GetCurrency() => _data.Currency;
}