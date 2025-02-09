using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private Image heartImg;
    [SerializeField] private GameObject imgContainer;
    [SerializeField] private LevelStarSprites levelStarSprites;
    [SerializeField] private float startDropHeight = 100;

    private Vector3 defaultPosition;

    private void Awake()
    {
        defaultPosition = imgContainer.transform.localPosition;
    }

    public void Active(bool isEnable)
    {
        heartImg.sprite = isEnable ? levelStarSprites.unlockSprite : levelStarSprites.lockSprite;
    }
    
    [Button]
    public void DropDown()
    {
        Vector3 startDropPosition = imgContainer.transform.localPosition;
        startDropPosition.y = startDropHeight;
        
        LMotion.Create(startDropPosition, defaultPosition,
                AnimationManager.Cur.config.topUIConfig.heartDropTime)
            .WithEase(AnimationManager.Cur.config.topUIConfig.heartDropEase)
            .WithDelay(AnimationManager.Cur.config.topUIConfig.heartDropDelay * transform.GetSiblingIndex() + 0.8f)
            .BindToLocalPosition(imgContainer.transform);
    }
}

