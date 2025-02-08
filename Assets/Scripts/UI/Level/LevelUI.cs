using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button lockedButton;

    [FormerlySerializedAs("playButton")] [SerializeField]
    private Button selectButton;

    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Sprite")] 
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite normalSprite;

    [Header("Animated")]
    [SerializeField] private Image radialShine;
    private int level;
    
    private RotatingImageUI rotatingImageUI;


    private void Awake()
    {
        defaultColor = radialShine.color;
        
        rotatingImageUI = GetComponent<RotatingImageUI>();
        rotatingImageUI.SetRotateImg(radialShine);
        Init(0, true);
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


    public void Init(int level, bool isUnlock)
    {
        this.level = level;
        levelText.text = $"{level + 1}";
        lockedButton.gameObject.SetActive(!isUnlock);
        selectButton.gameObject.SetActive(isUnlock);
    }

    public void Select()
    {
        SetSelect(true);
    }

    public void UnSelect()
    {
        SetSelect(false);
    }

    private void SetSelect(bool isSelect)
    {
        radialShine.gameObject.SetActive(isSelect);
        FadeRadialShine(isSelect);
        rotatingImageUI.SetEnable(isSelect);
    }

    [SerializeField] private float fadeTime = 0.25f;
    private Color defaultColor;

    private void FadeRadialShine(bool isFadeIn)
    {
        var startValue = !isFadeIn ? defaultColor : Color.clear;
        var endValue = !isFadeIn ? Color.clear : defaultColor;

        LMotion.Create(startValue, endValue, fadeTime).WithOnComplete(() =>
        {
            radialShine.color = endValue;
        }).BindToColor(radialShine);
    }

}