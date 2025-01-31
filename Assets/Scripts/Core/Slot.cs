using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using NaughtyAttributes;
using UnityEngine;

public enum SlotType
{
    Normal,
    Blocking,
    Explosion,
    Rewards
}

public class Slot : MonoBehaviour
{
    [SerializeField] private bool isEmpty = false;
    [SerializeField] private Tray currentTray;

    [Header("Settings")] [SerializeField] private float scaleUpTime = .25f;
    [SerializeField] private float scaleDownTime = .25f;
    [SerializeField] private float scaleValue = 1.3f;
   
    
    private Vector3 currentScale;
    private CompositeMotionHandle handles;

    public SlotType SlotType;
  
    public Action<Slot> PlacedCallback;

    private void Awake()
    {
        currentScale = transform.localScale;
        handles = new CompositeMotionHandle();
    }

    public virtual bool CanPlacedTray()
    {
        return isEmpty && SlotType == SlotType.Normal;
    }

    public void Add(Tray tray)
    {
        currentTray = tray;
        currentTray.SetTrayToSlot();
        // currentTray.transform.position = transform.position;
        isEmpty = false;
        LMotion.Create(currentTray.transform.position, transform.position, AnimationManager.Instance.AnimationConfig.releaseTrayDuration)
            .WithOnComplete(() => {PlacedCallback?.Invoke(this);} )
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

    public void PlayClearAnimation()
    {
        if (currentTray == null) return;
        StartCoroutine(DestroyTrayWithDelay());
    }

    private IEnumerator  DestroyTrayWithDelay()
    {
        bool isDelay = false;
        bool canDestroy = false;
        if (currentTray.items.Count == 0)
        {
            canDestroy = true;
        }
        else if(currentTray.IsFullOfItem(out var itemID))
        {
            canDestroy = true;
            isDelay = true;
            
            PuzzleQuestManager.Instance?.OnCompleteItem(itemID);
        }

        if (canDestroy)
        {
            yield return new WaitForSeconds( isDelay ? AnimationManager.Instance.AnimationConfig.clearTrayDelay : 0.1f);
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
  
 
}