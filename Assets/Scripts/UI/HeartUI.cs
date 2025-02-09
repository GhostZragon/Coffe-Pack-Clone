using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private Image heartImg;
    [SerializeField] private LevelStarSprites levelStarSprites;

    public void Active(bool isEnable)
    {
        heartImg.sprite = isEnable ? levelStarSprites.unlockSprite : levelStarSprites.lockSprite;
    }
}
