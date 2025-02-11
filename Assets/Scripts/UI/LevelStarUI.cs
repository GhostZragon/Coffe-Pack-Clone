using UnityEngine;
using UnityEngine.UI;

public class LevelStarUI : MonoBehaviour
{
    [SerializeField] private Image[] images;

    [SerializeField] private LevelStarSprites levelStarSprites;

    public void ActiveStageUnlock(int count)
    {
        count = Mathf.Clamp(count, 0, 3);

        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = i <= count - 1 ? levelStarSprites.unlockSprite : levelStarSprites.lockSprite;
        }
    }

    public Vector3 GetStarByIndex(int starIndex)
    {
        return images[starIndex].transform.position;
    }
}