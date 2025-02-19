using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameplayUI gameplayUI;
    public MenuUI menuUI;

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


