using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button lockedButton;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI levelText;
    private int level;
    private void Awake()
    {
        Setup(0, true);
    }

    public void Setup(int level, bool isUnlock)
    {
        this.level = level;
        levelText.text = level.ToString();
        lockedButton.gameObject.SetActive(isUnlock);
        playButton.gameObject.SetActive(!isUnlock);
    }
}
