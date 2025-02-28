using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameResult
{
    Win,
    Lose
}
public readonly struct ResultData
{
    public int StarUnlocked { get; }
    public int CoinReward { get; }
    public GameResult Result { get; }

    public ResultData(int star, int coin, GameResult _result)
    {
        StarUnlocked = star;
        CoinReward = coin;
        Result = _result;
    }
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
        var popup = CreatePopup(resultData);
        popup.Show(resultData);
        return popup;
    }

    private ResultUI CreatePopup(ResultData resultData)
    {
        GameObject prefab = resultData.Result == GameResult.Win ? winResultPopupPrefab : losseResultPopupPrefab;
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

    public void Reload()
    {
        QuestStageUI.ResetProgressUI();
    }
}