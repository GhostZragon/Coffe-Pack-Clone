using System;
using UnityEngine;

public class EventManager
{
    private static EventManager current;

    private EventManager()
    {
        _Game = new();
        _UI = new UI();
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

    public Game _Game;
    public UI _UI;
    public class Game
    {
        public Action<int> OnSelectLevel;
        public Action OnLoadLevel;
        public Action OnUnloadLevel;
        public Action CheckWin;
        public Action CheckLoose;
        public Action OnProcessComplete;
    }

    public class UI
    {
        public Action<LevelUI> OnSelectLevelUI;
        public Action<InGameQuestData> OnBindingWithQuestUI;
    }
}