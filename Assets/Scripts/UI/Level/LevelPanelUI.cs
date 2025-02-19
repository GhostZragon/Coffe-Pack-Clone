using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private MapLevelUI[] mapLevelUis;
    [SerializeField] private LevelUI levelUIPrefab;

    public Predicate<int> levelUnlockChecking;
    public GameObject avatarAnchor;


    private LevelUI previousLevelUI;
    public void SelectLevelUI(int level)
    {
        foreach (var levelMap in mapLevelUis)
        {
            previousLevelUI?.UnSelect();
            
            if (!levelMap.TryGetLevelUI(level, out previousLevelUI)) continue;
            previousLevelUI.Select();
            
            avatarAnchor.transform.SetParent(levelMap.transform);
            avatarAnchor.transform.localPosition = previousLevelUI.transform.localPosition;
            
            Debug.Log($"Select UI, active effect, Parent{levelMap.transform.name}", levelMap.gameObject);
            
            break;
        }
    }

    public void Initialize(int maxLevel)
    {
        int startLevel = 0;
        foreach (var mapLevel in mapLevelUis)
        {
            if (startLevel >= maxLevel)
                break;
            if(mapLevel.CanInit())
                mapLevel.InitLevelUIs(levelUIPrefab);
            
            mapLevel.SetRangeOfLevel(ref startLevel, maxLevel, levelUnlockChecking);
        }
    }

 


#if UNITY_EDITOR
    [Header("Editor Only")]
    public GameObject map;
    public GameObject newParent;

    [Button]
    private void CreateLevelSetup()
    {
        foreach (Transform item in map.transform)
        {
            var go = new GameObject("Level Spawn", typeof(RectTransform));
            go.transform.parent = newParent.transform;
            go.transform.localPosition = item.transform.localPosition;
        }
    }

#endif
}