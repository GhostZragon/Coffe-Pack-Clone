using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    [SerializeField] private Image emoji;
    [SerializeField] private TextMeshProUGUI emojiText;
    [SerializeField] private TextMeshProUGUI coinText;
    
    public LevelResultStarUI LevelResultStarUI;



    public void BackToMenu()
    {
        EventManager.Current._Core.OnUnloadLevel?.Invoke();
        UIManager.Instance.ShowMenuUI();
        Destroy(gameObject);
    }

    public void ReplayGame()
    {
        EventManager.Current._Core.OnReloadGame?.Invoke();
        Destroy(gameObject);
    }
    
    
    public void Show(ResultData resultData)
    {
        Debug.Log("Show Result UI");
        gameObject.SetActive(true);
        
        if(resultData.IsWin)
            LevelResultStarUI.ActiveStageUnlock(resultData.starUnlocked);
    }

   
    
    public void Hide()
    {
        Debug.Log("Hide result UI");
        gameObject.SetActive(false);
    }
}
