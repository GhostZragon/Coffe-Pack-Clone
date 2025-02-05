using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button lockedButton;
    [FormerlySerializedAs("playButton")] [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite normalSprite;
    private int level;
    private void Awake()
    {
        Setup(0, true);
    }

    private void OnEnable()
    {
        selectButton.onClick.AddListener(OnSelectLevel);
    }

    private void OnDisable()
    {
        selectButton.onClick.RemoveListener(OnSelectLevel);
    }

    private void OnSelectLevel()
    {
        Debug.Log("Select Level");
        
        EventManager.Current._Core.OnSelectLevel(level);
    }
    

    public void Setup(int level, bool isUnlock)
    {
        this.level = level;
        levelText.text = $"{level + 1}";
        lockedButton.gameObject.SetActive(!isUnlock);
        selectButton.gameObject.SetActive(isUnlock);
    }
    
}
