using UnityEngine;

public class CollectorManager : MonoBehaviour
{
    [SerializeField] private CollectorUI collectorUI;

    [SerializeField] private int maxPercent = 100;
    [SerializeField] private float currentPercent = 0;

    private void Awake()
    {
        CollectEffect_CoinCollector.OnCompleteEffect += AddNewPercent;
    }

    private void OnDestroy()
    {
        CollectEffect_CoinCollector.OnCompleteEffect -= AddNewPercent;
    }

    private void AddNewPercent()
    {
        currentPercent += Random.Range(1, 3);
        collectorUI.FillAmount(currentPercent / maxPercent);
    }
}