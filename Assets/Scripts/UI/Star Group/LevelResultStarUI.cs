using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultStarUI : LevelStarUIBase
{

    [SerializeField] private GameObject starTemp;
    [SerializeField] private Transform[] starPositions;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float delayActivePerStar = 0.3f;
    private void Awake()
    {
        starTemp.gameObject.SetActive(false);
    }

    [Button]
    public override void ActiveStageUnlock(int count)
    {
        base.ActiveStageUnlock(count);
    }


    protected override void ActiveStar(int index, bool isUnlock)
    {
        base.ActiveStar(index,isUnlock);
        // if (isUnlock)
        // {
        //     PlayActiveStarEffect(index);
        // }
        // else
        // {
        //     images[index].sprite = levelStarSprites.lockSprite;
        // }
    }
    
    [Button]
    private void PlayActiveStarEffect(int index)
    {
        images[index].sprite = levelStarSprites.lockSprite;
        
        var starTransform = images[index].transform;
        var startSpawnPoint = starPositions[index];
        var starEffect = Instantiate(starTemp, startSpawnPoint.position, Quaternion.identity,transform);
        var delayTime = (index + 1) * delayActivePerStar;
        starEffect.gameObject.SetActive(true);
            
        LMotion.Create(1f, 0f, fadeDuration)
            .WithDelay(delayTime)
            .BindToColorA(starEffect.GetComponent<Image>());
        LMotion.Create(starEffect.transform.position, starTransform.position, moveDuration)
            .WithDelay(delayTime)
            .WithOnComplete(() =>
            {
                Destroy(starEffect.gameObject);
                images[index].sprite = levelStarSprites.unlockSprite;
            })
            .BindToPosition(starEffect.transform);
    }

    public override void SetMaxStar(int count)
    {
        for (int i = 1; i <= images.Length; i++)
        {
            images[i].gameObject.SetActive(i <= count);
        }
    }
}