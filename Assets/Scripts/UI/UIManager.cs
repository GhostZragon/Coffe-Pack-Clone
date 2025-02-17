using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameplayUI gameplayUI;
    [SerializeField] private MenuUI menuUI;

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


