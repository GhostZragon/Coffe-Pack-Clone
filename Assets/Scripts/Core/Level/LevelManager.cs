using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private LevelPanelUI levelPanelUI;
    [SerializeField] private bool startQuestByButton = false;

    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    
    
    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;
    private DragDropSystem dragDropSystem;
    private TrayManager trayManager;
    
    private void Awake()
    {
        currentLevel = 0;
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        dragDropSystem = FindFirstObjectByType<DragDropSystem>();
        trayManager = FindFirstObjectByType<TrayManager>();

        levelConfigs = Resources.LoadAll<LevelConfig>("Level");

        levelPanelUI.levelUnlockChecking = IsLevelUnlock;
        
        EventManager.Current._Game.OnLoadLevel += LoadLevel;
        EventManager.Current._Game.OnUnloadLevel += UnLoadLevel;
        EventManager.Current._Game.OnSelectLevel += SetLevel;

        EventManager.Current._Game.OnProcessComplete += OnProcessComplete;

    }

    private void OnDestroy()
    {
        levelPanelUI.levelUnlockChecking = null;
        
        EventManager.Current._Game.OnLoadLevel -= LoadLevel;
        EventManager.Current._Game.OnUnloadLevel -= UnLoadLevel;
        EventManager.Current._Game.OnSelectLevel -= SetLevel;
        
        EventManager.Current._Game.OnProcessComplete -= OnProcessComplete;


    }

    private void Start()
    {
        if (levelConfig == null)
        {
            levelConfig = levelConfigs[currentLevel];
        }

        if (startQuestByButton == false)
        {
            LoadLevel();
        }
        
        SettingsLevel();
    }

    private void SettingsLevel()
    {
        maxLevel = levelConfigs.Length - 1;
        currentLevel = Mathf.Clamp(currentLevel,0,levelConfigs.Length);

        levelPanelUI.Init(maxLevel);
        
        EventManager.Current._Game.OnSelectLevel?.Invoke(currentLevel);
    }

    private void LoadLevel()
    {
        gridManager.SetLevelData(levelConfig.LevelCSV);
        gridManager.InitializeGrid();
        
        puzzleQuestManager.SetPuzzleQuestData(levelConfig.PuzzleQuestData);
        puzzleQuestManager.SetFirstState();
        puzzleQuestManager.CreateQuests();

        trayManager.Initialize();
        UIManager.Instance.ShowGameplayUI();
    }


    private void UnLoadLevel()
    {
        gridManager.ClearGrid();
        puzzleQuestManager.ClearQuest();
        trayManager.ClearAllTray();
        dragDropSystem.ClearDragItem();
    }

    private void SetLevel(int levelIndex)
    {
        this.currentLevel = levelIndex;
        levelConfig = levelConfigs[currentLevel];
    }

    private void OnProcessComplete()
    {
        
        // check win loose
        trayManager.TryCreateNextTrays();
    }

    private bool IsLevelUnlock(int i)
    {
        return i >= currentLevel;
    }
}

