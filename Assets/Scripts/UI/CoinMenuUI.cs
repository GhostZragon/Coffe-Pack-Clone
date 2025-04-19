using TMPro;
using UnityEngine;

public class CoinMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    GameDataController gameDataController;
    private void OnEnable()
    {
        Valid();

        gameDataController.OnCurrencyChanged += CoinChanged;
        CoinChanged(gameDataController.GetCurrency());
    }

    private void OnDisable()
    {
        Valid();

        gameDataController.OnCurrencyChanged -= CoinChanged;
    }

    private void Valid()
    {
        if (gameDataController == null)
            gameDataController = DataManager.Instance.GetDataController<GameDataController>();
    }

    public void CoinChanged(int coin)
    {
        coinText.text = coin.ToString();
    }
}
