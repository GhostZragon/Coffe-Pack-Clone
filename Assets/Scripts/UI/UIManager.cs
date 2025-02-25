using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;

        foreach (var view in GetComponentsInChildren<BaseView>())
        {
            view.Initialize();
        }
        pauseUI.Hide();
    }

    public GameplayUI gameplayUI;
    public MenuUI menuUI;
    public PauseUI pauseUI;
    public void ShowGameplayUI()
    {
        gameplayUI.Show();
        menuUI.Hide();
    }

    public void ShowMenuUI()
    {
        gameplayUI.Hide();
        menuUI.Show();
    }
    
}


