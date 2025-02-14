using System;
using LitMotion;
using UnityEngine;

public class CollectEffect_CoinCollector : QuestCollectEffectBase
{
    [SerializeField] private CollectorUI collectorUI;
    public static event Action OnCompleteEffect;
    public override void CreateTrail(Vector3 spawnPos)
    {
        var effect = Instantiate(trailPrefab, spawnPos, Quaternion.identity, transform);
        effect.gameObject.SetActive(true);
        var custom = BezierCustom.Create(effect.transform.position, collectorUI.GetIconPosition());

        LMotion.Create(0f, 1f, 1)
            .WithOnComplete(() =>
            {
                OnCompleteEffect?.Invoke();
                Destroy(effect.gameObject);
            })
            .Bind((x) =>
            {
                custom.Play3(x);
                effect.transform.position = custom.moveObjectPosition;
            });
    }

   
}