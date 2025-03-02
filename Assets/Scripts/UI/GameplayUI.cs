using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameResult
{
    Win,
    Lose
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

    public ResultUI ShowResultMenu()
    {
        var data = DataManager.Instance.GetDataController<GameSessionController>().Data;

        panel.gameObject.SetActive(true);
        var popup = CreatePopup(data.GameResult);
        popup.Show();
        return popup;
    }

    private ResultUI CreatePopup(GameResult gameResult)
    {
        GameObject prefab = gameResult == GameResult.Win ? winResultPopupPrefab : losseResultPopupPrefab;
        var resultUI = Instantiate(prefab, transform).GetComponent<ResultUI>();
        resultUI.Initialize();
        return resultUI;
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