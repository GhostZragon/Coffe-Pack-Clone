using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Button backMenuButton;
    public Button replayButton;

    public LevelResultStarUI LevelResultStarUI;
    
    public void Show(ResultData resultData)
    {
        Debug.Log("Show Result UI");
        gameObject.SetActive(true);
        
        LevelResultStarUI.ActiveStageUnlock(resultData.starUnlocked);
    }

    public void Hide()
    {
        Debug.Log("Hide result UI");
        gameObject.SetActive(false);
    }
}
