using LitMotion;
using LitMotion.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    [SerializeField] private LevelResultStarUI LevelResultStarUI;
    [SerializeField] private Image emoji;
    [SerializeField] private TextMeshProUGUI emojiText;
    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI rewardCoinText;


    public Action OnBackMenuClicked;
    public Action OnReplayClicked;

    private GameSessionController gameSessionController;


    public void Initialize()
    {
        gameSessionController = DataManager.Instance.GetDataController<GameSessionController>();
    }

    public void BackToMenu()
    {
        OnBackMenuClicked();
        Destroy(gameObject);

    }

    public void ReplayGame()
    {
        OnReplayClicked();
        Destroy(gameObject);
    }
    
    
    public void Show()
    {
        Debug.Log("Show Result UI");
        gameObject.SetActive(true);

        if(gameSessionController == null)
        {
            Debug.LogError("Game session controller is null", gameObject);
            return;
        }

        var data = gameSessionController.Data;

        if(data.GameResult == GameResult.Win)
            LevelResultStarUI.ActiveStageUnlock(data.StarUnlocked);
    }
    public CompositeMotionHandle motionHandles;
    public void UpdateRewardCoinText(int rewardCoin)
    {
        // increase total coin and decrease reward coin
        var handle = LMotion.Create(0, rewardCoin, 1)
            .BindToText(rewardCoinText);

        motionHandles.Add(handle);
    }

    public void Hide()
    {
        Debug.Log("Hide result UI");
        gameObject.SetActive(false);
    }

    
}
