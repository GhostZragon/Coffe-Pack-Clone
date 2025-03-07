using UnityEngine;
using UnityEngine.UI;

public abstract class LevelStarUIBase : MonoBehaviour
{
    [SerializeField] protected Image[] images;

    [SerializeField] protected LevelStarSprites levelStarSprites;
    public virtual void ActiveStageUnlock(int count)
    {
        count = Mathf.Clamp(count, 0, 3);

        for (int i = 0; i < images.Length; i++)
        {
            ActiveStar(i, i <= count - 1);
        }
    }

    protected virtual void ActiveStar(int index, bool isUnlock)
    {
        images[index].sprite = isUnlock ? levelStarSprites.unlockSprite : levelStarSprites.lockSprite;
    }

    public Vector3 GetTargetPositionByIndex(int starIndex)
    {
        return images[starIndex].transform.position;
    }

    public virtual void SetMaxStar(int count)
    {

    }
}
