﻿using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SlotBase : MonoBehaviour
{
    [SerializeField] protected Transform model;
    public abstract bool CanPlacedTray();
    public abstract void ActiveSpecialEffect();

    public abstract void PlayClearAnimation();
    
    private MotionHandle sequenceHandler;

    public void InitEffect(float delay, Ease ease)
    {
        Vector3 newPosition = model.localPosition;
        float height = model.localPosition.y + 40;
        newPosition.y = height;

        var originalScale = model.transform.localScale;
       
        // play animation
        
        sequenceHandler = LSequence.Create()
            .Append(LMotion.Create(newPosition, Vector3.zero, AnimationManager.Cur.config.slotCfg.normalSlotDropTime)
                .WithEase(ease)
                .WithDelay(delay)
                .BindToLocalPosition(model)
                .AddTo(model))
            .AppendInterval(0.1f)
            .Append(LMotion.Create(originalScale, originalScale * 1.2f, 0.1f)
                .BindToLocalScale(model).AddTo(model))
            .AppendInterval(0.1f)
            .Append(LMotion.Create(model.transform.localScale, originalScale, 0.1f)
                .BindToLocalScale(model).AddTo(model))
            .Run();

        sequenceHandler.AddTo(model);
    }

    public void SetSize(float widthSize,float heighSize)
    {
        transform.localScale = new Vector3(widthSize, transform.localScale.y, heighSize);
    }

    protected virtual void OnDestroy()
    {
        sequenceHandler.TryCancel();
    }
}

