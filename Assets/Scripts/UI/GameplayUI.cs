using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(BackToMenuUI);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(BackToMenuUI);
    }

    private void BackToMenuUI()
    {
        UIManager.Instance.ShowMenuUI();
        EventManager.Current._Core.OnUnloadLevel?.Invoke();
    }
}
