using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private bool isEmpty = false;
    [SerializeField] private Tray currentTray;
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
    }

    public void Remove()
    {
        currentTray = null;
    }
}
