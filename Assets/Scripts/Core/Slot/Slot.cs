using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SlotType
{
    Normal,
    Blocking,
    Explosion,
    Rewards
}

public class Slot : SlotBase
{
    [SerializeField] protected bool isEmpty = false;
    [SerializeField] protected Tray currentTray;

    [Header("Settings")] [SerializeField] private float scaleUpTime = .25f;
    [SerializeField] private float scaleDownTime = .25f;
    [SerializeField] private float scaleValue = 1.3f;


    private Vector3 currentScale;
    private CompositeMotionHandle handles;

    protected void Awake()
    {
        currentScale = transform.localScale;
        handles = new CompositeMotionHandle();
    }

    private void OnPlacedTray()
    {
        Table.Instance.mergeSystem.TryMergeAtSlot(this);
    }

    public void Add(Tray tray)
    {
        currentTray = tray;
        currentTray.SetTrayToSlot();
        // currentTray.transform.position = transform.position;
        isEmpty = false;
        LMotion.Create(currentTray.transform.position, transform.position,
                AnimationManager.Instance.config.trayCfg.releaseTrayDuration)
            .WithOnComplete(OnPlacedTray)
            .BindToPosition(currentTray.transform);
        // AnimationManager.Instance.MoveTrayToSlot(currentTray.transform, transform.position, placedCallback);
    }

    [Button]
    public void OnSelect()
    {
        ScaleSlot(scaleUpTime, transform.localScale, currentScale * scaleValue);
    }

    [Button]
    public void UnSelect()
    {
        ScaleSlot(scaleDownTime, transform.localScale, currentScale);
    }

    private void ScaleSlot(float time, Vector3 currentScale, Vector3 targetScale)
    {
        handles.Cancel();
        var motions = LMotion.Create(currentScale, targetScale, time).BindToLocalScale(transform).AddTo(gameObject);
        handles.Add(motions);
    }

    public Tray GetTray()
    {
        return currentTray;
    }

    public override void PlayClearAnimation()
    {
        if (currentTray == null) return;
        StartCoroutine(DestroyTrayWithDelay());
    }

    private IEnumerator DestroyTrayWithDelay()
    {
        bool isDelay = false;
        bool canDestroy = false;
        if (currentTray.items.Count == 0)
        {
            canDestroy = true;
        }
        else if (currentTray.IsFullOfItem(out var itemID))
        {
            canDestroy = true;
            isDelay = true;
            Table.Instance.OnCompleteItem(this);
            PuzzleQuestManager.Instance?.OnCompleteItem(itemID);
        }

        if (canDestroy)
        {
            yield return new WaitForSeconds(isDelay ? AnimationManager.Instance.config.trayCfg.clearTrayDelay : 0.1f);
            ClearTray();
        }

        yield return null;
    }


    [Button]
    public void ClearTray()
    {
        Debug.Log("Clear Tray and destroy it");
        currentTray?.DestroyAnimation();
        currentTray = null;
        isEmpty = true;
    }


    public override bool CanPlacedTray()
    {
        return isEmpty;
    }

    public override void ActiveSpecialEffect()
    {
    }
}