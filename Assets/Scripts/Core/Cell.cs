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
            Debug.Log("Đã tìm thấy slot");
            return slot.GetTray();
        }
        Debug.Log("không tìm thấy slot");
        return null;
    }
}