using UnityEngine;
using UnityEngine.UI;

public class ResultData
{
    public ResultData(int star,int coin)
    {
        starUnlocked = star;
        coinReward = coin;
    }
    
    public int starUnlocked;
    public int coinReward;
}
public class GameplayUI : BaseView
{
    [SerializeField] private Button backButton;
    [SerializeField] private ResultUI resultUI;
  

    protected override void Register()
    {
        backButton.onClick.AddListener(BackToMenuUI);
        
        resultUI.backMenuButton.onClick.AddListener(BackToMenuUI);
        resultUI.replayButton.onClick.AddListener(ReplayGame);
        
        EventManager.Current._UI.OnShowResultUI += ShowResultMenu;
    }

    protected override void UnRegister()
    {
        backButton.onClick.RemoveListener(BackToMenuUI);
      
        resultUI.backMenuButton.onClick.RemoveListener(BackToMenuUI);
        resultUI.replayButton.onClick.RemoveListener(ReplayGame);

        EventManager.Current._UI.OnShowResultUI -= ShowResultMenu;
    }
  
    private void BackToMenuUI()
    {
        UIManager.Instance.ShowMenuUI();
        EventManager.Current._Core.OnUnloadLevel?.Invoke();
        resultUI.Hide();
    }

    private void ShowResultMenu(ResultData ResultData)
    {
        resultUI.Show(ResultData);
    }

    public override void Show()
    {
        base.Show();
        resultUI.Hide();
    }

    public override void Hide()
    {
        base.Hide(); 
        resultUI.Hide();
    }



    private void ReplayGame()
    {
        EventManager.Current._Core.OnReloadGame?.Invoke();
    }
}