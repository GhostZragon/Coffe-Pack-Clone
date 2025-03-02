using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    [SerializeField] private Image emoji;
    [SerializeField] private TextMeshProUGUI emojiText;
    [SerializeField] private TextMeshProUGUI coinText;
    
    [SerializeField] private LevelResultStarUI LevelResultStarUI;

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

   
    
    public void Hide()
    {
        Debug.Log("Hide result UI");
        gameObject.SetActive(false);
    }

    
}
