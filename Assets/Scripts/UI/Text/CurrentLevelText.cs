using System;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CurrentLevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmPro;
    private Color defaultColor;


    private Vector3 defaultPosition;
    private Vector3 upPosition;
    private Vector3 downPosition;
    
    private void Awake()
    {
        EventManager.Current._Core.OnSelectLevel += OnSelectLevel;
        defaultColor = tmPro.color;
     
        defaultPosition = tmPro.transform.localPosition;
   
        upPosition = defaultPosition;
        upPosition.y += 100;
        
        downPosition = defaultPosition;
        downPosition.y -= 100;

        UpDownMovement();
    }

    private void UpDownMovement()
    {
        Vector3 defaultPos = transform.localPosition;

        LMotion.Create(defaultPos, defaultPos + new Vector3(0, 10, 0), 1)
            .WithEase(Ease.InOutSine)
            .WithLoops(-1,LoopType.Yoyo)
            .BindToLocalPosition(transform);
    }
    
    private void OnDestroy()
    {
        EventManager.Current._Core.OnSelectLevel -= OnSelectLevel;
    }

    private void OnSelectLevel(int level)
    {
        UseEffect2(level);
    }
    
    [Button]
    private void UseEffect2(int level)
    {
        
        LMotion.Create(defaultPosition, upPosition, 0.3f)
            .WithEase(Ease.InQuad)
            .WithOnComplete(() =>
            {
                tmPro.text = $"{level}";
                tmPro.transform.localPosition = downPosition;
                LMotion.Create(downPosition, defaultPosition, .3f)
                    .WithEase(Ease.OutQuad).BindToLocalPosition(tmPro.transform);

            }).BindToLocalPosition(tmPro.transform);
    }
    
    [Button]
    private void UseEffect()
    {
        LMotion.Create(Vector3.one, Vector3.one * 1.2f, 0.2f)
            .WithEase(Ease.OutQuad)
            .BindToLocalScale(tmPro.transform);
        LMotion.Create(defaultColor, Color.clear, 0.2f)
            .WithEase(Ease.OutQuad)
            .WithOnComplete(() =>
            {
                LMotion.Create(Color.clear, defaultColor, 0.2f).BindToColor(tmPro);
                
                LMotion.Create(Vector3.one * 1.2f, Vector3.one, 0.2f)
                    .BindToLocalScale(tmPro.transform);
            })
            .BindToColor(tmPro);
    }
}
