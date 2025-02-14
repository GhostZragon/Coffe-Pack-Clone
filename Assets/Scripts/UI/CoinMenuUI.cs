using TMPro;
using UnityEngine;

public class CoinMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        EventManager.Current._Game.OnCoinChanged += CoinChanged;
    }

    private void OnDestroy()
    {
        EventManager.Current._Game.OnCoinChanged -= CoinChanged;
    }


    private void CoinChanged(int coin)
    {
        coinText.text = coin.ToString();
    }
}
