using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button playButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        Debug.Log("Start game");
    }

    private void Update()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }
}
