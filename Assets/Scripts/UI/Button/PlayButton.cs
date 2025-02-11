using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button button;
  
    private void Awake()
    {
        button.onClick.AddListener(Play);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Play);
    }

    private void Play()
    {
        EventManager.Current._Core.OnLoadLevel?.Invoke();
    }
}