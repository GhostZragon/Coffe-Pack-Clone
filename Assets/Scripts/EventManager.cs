using System;
using UnityEngine;

public class EventManager
{
    private static EventManager current;

    private EventManager()
    {
        _Core = new();
        _UI = new();
        _Game = new();
        _Table = new();
    }
    public static EventManager Current
    {
        get
        {
            if (current == null)
            {
                current = new();
            }

            return current;
        }
    }

    public readonly Core _Core;
    public readonly UI _UI;
    public readonly Game _Game;
    public readonly Table _Table;
    public class Core
    {
        public Action<int> OnSelectLevel;
        public Action OnLoadLevel;
        public Action OnUnloadLevel;
        public Action CheckWin;
        public Action CheckLoose;
        public Action OnProcessComplete;
    }

    public class Game
    {
        public Action<Slot> OnMergeTray;
        
        public Action<string> OnCompleteItem;
        
        public Action<int> OnCoinChanged;
    }

    public class Table
    {
        public Action<SlotBase, SlotBase> OnReplaceSlot;
        public Action<SlotBase> OnDestroyBlockingBlockAround;

    }

    public class UI
    {
        public Action<InGameQuestData> OnBindingWithQuestUI;
        
    }
}

public class ItemInfo
{
    public string ItemId;
    public Vector3 TrayWorldPosition;
}