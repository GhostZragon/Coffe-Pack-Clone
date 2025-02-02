using System;
using UnityEngine;

[Serializable]
public class Cell
{

    public SlotBase Slot { get; private set; }
    public Tray Tray => GetTray();
    public bool HasTray => Tray != null;
    public bool HasSlot => Slot != null;
    public Cell(SlotBase slot) => Slot = slot;

    public void SetSlot(Slot slot)
    {
        Slot = slot;
    }

    private Tray GetTray()
    {
        if (Slot is Slot slot)
        {
            return slot.GetTray();
        }

        return null;
    }
}