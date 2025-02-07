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

    private void Awake()
    {
        EventManager.Current._Core.OnSelectLevel += SelectLevelUI;
    }

    private void OnDestroy()
    {
        EventManager.Current._Core.OnSelectLevel -= SelectLevelUI;
    }

    private LevelUI previousLevelUI;
    private void SelectLevelUI(int level)
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

    public void Init(int maxLevel)
    {
        int startLevel = 0;


        foreach (var mapLevel in mapLevelUis)
        {
            // Get spawn point            
            var spawnPoints = mapLevel.GetAllSpawnPoints();
            var levelUis = new LevelUI[spawnPoints.Length];
            // create level array
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                levelUis[i] = Instantiate(levelUIPrefab, spawnPoints[i].position, Quaternion.identity,
                    mapLevel.transform);
            }

            mapLevel.InitLevelUIs(levelUis);

            if (startLevel >= maxLevel)
                break;

            mapLevel.InitMap(ref startLevel, maxLevel, IsLevelUnlock);
        }
    }

    private bool IsLevelUnlock(int i)
    {
        return levelUnlockChecking(i);
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