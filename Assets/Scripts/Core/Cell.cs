using System;
using UnityEngine;

[Serializable]
public class Cell
{

    public Slot Slot { get; private set; }
    public Tray Tray => Slot.GetTray();
    public bool HasTray => Tray != null;
    
    public Cell(Slot slot) => Slot = slot;
}