using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelStarProgressUI : LevelStarUIBase
{
    [SerializeField] private int Frequency = 2;
    [SerializeField] private int Damping_Ratio  = 10;
    [SerializeField] private int maxStage;
    [SerializeField] private List<Image> lists = new();
  

    [Button]
    public override void ActiveStageUnlock(int count)
    {
        for (int i = 0; i < lists.Count; i++)
        {
            Sprite sprite = null;
            if (i < count )
            {
                sprite = levelStarSprites.unlockSprite;
            }
            else
            {
                sprite = levelStarSprites.lockSprite;
            }

            lists[i].sprite = sprite;
        }
    }

    public void CollectPointEffect(int currentStage)
    {
        var starTransform = images[currentStage].transform;

        LMotion.Punch.Create(Vector3.one,Vector3.one * 1.05f, 0.2f)
            .WithFrequency(Frequency)
            .WithDampingRatio(Damping_Ratio)
            .BindToLocalScale(starTransform);
    }
    [Button]
    public override void SetMaxStar(int count)
    {
        this.maxStage = count;
        lists.Clear();
        for (int i = images.Length - 1; i >= 0; i--)
        {
            // Tính toán chỉ số bắt đầu từ phần tử cuối mảng
            bool isActive = i >= (images.Length - count);
            images[i].gameObject.SetActive(isActive);
            
            if(isActive)
                lists.Add(images[i]);
        }

        lists.Reverse();
    }
}