using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultData
{
    public ResultData(int star,int coin, bool isWin)
    {
        starUnlocked = star;
        coinReward = coin;
        IsWin = isWin;
    }
    
    public int starUnlocked;
    public int coinReward;
    public bool IsWin;
}
public class GameplayUI : BaseView
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject losseResultPopupPrefab;
    [SerializeField] private GameObject winResultPopupPrefab;
    [SerializeField] private GameObject panel;
    public QuestStageUI QuestStageUI;
    public PuzzleQuestManagerUI PuzzleQuestManagerUI;
    public Action OpenPauseMenuClicked;
  
    protected override void Register()
    {
        backButton.onClick.AddListener(OpenMouseMenu);
        panel.gameObject.SetActive(false);
    }

    protected override void UnRegister()
    {
        backButton.onClick.RemoveListener(OpenMouseMenu);

    }
  
    private void OpenMouseMenu()
    {
        OpenPauseMenuClicked?.Invoke();
    }

    public ResultUI ShowResultMenu(ResultData resultData)
    {
        panel.gameObject.SetActive(true);
        var popup = CreatePopup(resultData.IsWin);
        popup.Show(resultData);
        return popup;
    }

    private ResultUI CreatePopup(bool isWin)
    {
        GameObject prefab = null;
        prefab = isWin ? winResultPopupPrefab : losseResultPopupPrefab;
        return Instantiate(prefab, transform).GetComponent<ResultUI>();
    }

    public override void Show()
    {
        base.Show();
        panel.gameObject.SetActive(false);

    }

    public override void Hide()
    {
        base.Hide(); 
        panel.gameObject.SetActive(false);

    }

}