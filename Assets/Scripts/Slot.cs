using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using NaughtyAttributes;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private bool isEmpty = false;
    [SerializeField] private Tray currentTray;
    
    [Header("Settings")]
    [SerializeField] private float scaleUpTime = .25f;
    [SerializeField] private float scaleDownTime = .25f;
    [SerializeField] private float scaleValue = 1.3f;
    
    private Vector3 currentScale;
    private CompositeMotionHandle handles;
    
    private void Awake()
    {
        currentScale = transform.localScale;
        handles = new CompositeMotionHandle();
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void SetEmpty(bool _isEmpty)
    {
        isEmpty = _isEmpty;
    }

    public void Add(Tray tray)
    {
        currentTray = tray;
        currentTray.SetTrayToSlot();
        currentTray.transform.position = transform.position;
    }

    [Button]
    public void RemoveCurrentTray()
    {
        isEmpty = true;
        if(currentTray != null)
            Destroy(currentTray.gameObject);
    }

    
    [Button]
    public void OnSelect()
    {
        ScaleSlot(scaleUpTime,transform.localScale, currentScale *scaleValue);
    }

    [Button]
    public void UnSelect()
    {
        ScaleSlot(scaleDownTime,transform.localScale, currentScale);
    }

    private void ScaleSlot(float time, Vector3 currentScale, Vector3 targetScale)
    {
        handles.Cancel();
        var motions = LMotion.
            Create(currentScale, targetScale, time).
            BindToLocalScale(transform).
            AddTo(gameObject);
        handles.Add(motions);
    }

    public Tray GetTray()
    {
        return currentTray;
    }

    public bool TryGetTray(out Tray tray)
    {
        tray = currentTray;
        return currentTray != null;
    }
}