using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject menuUI;

    public void ShowGameplayUI()
    {
        gameplayUI.gameObject.SetActive(true);
        menuUI.gameObject.SetActive(false);
    }

    public void ShowMenuUI()
    {
        gameplayUI.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(true);
    }
    
}
