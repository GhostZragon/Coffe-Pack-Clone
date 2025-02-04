using System;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private MapLevelUI[] mapLevelUis;
    public Predicate<int> levelUnlockChecking;
    public GameObject avatarAnchor;

    private void Awake()
    {
        EventManager.Current._UI.OnSelectLevelUI += SelectLevelUI;
    }

    private void OnDestroy()
    {
        EventManager.Current._UI.OnSelectLevelUI -= SelectLevelUI;

    }

    private void SelectLevelUI(LevelUI levelUI)
    {
        avatarAnchor.transform.position = levelUI.transform.position;
    }
    public void Init(int maxLevel)
    {
        int startLevel = 0;

        foreach (var mapLevel in mapLevelUis)
        {
            if (startLevel >= maxLevel)
                break;
            mapLevel.RefreshMap(ref startLevel, maxLevel, IsLevelUnlock);
        }

    }

    private bool IsLevelUnlock(int i)
    {
        return levelUnlockChecking(i);
    }
    
    
}