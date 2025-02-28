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
    
    
    public void Show(ResultData resultData)
    {
        Debug.Log("Show Result UI");
        gameObject.SetActive(true);
        
        if(resultData.Result == GameResult.Win)
            LevelResultStarUI.ActiveStageUnlock(resultData.StarUnlocked);
    }

   
    
    public void Hide()
    {
        Debug.Log("Hide result UI");
        gameObject.SetActive(false);
    }
}
