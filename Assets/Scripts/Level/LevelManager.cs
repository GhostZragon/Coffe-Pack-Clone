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

    
    private void Awake()
    {
        currentLevel = 0;
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        levelConfigs = Resources.LoadAll<LevelConfig>("Level");

        levelPanelUI.levelUnlockChecking = IsLevelUnlock;
    }

    private void OnDestroy()
    {
        levelPanelUI.levelUnlockChecking = null;
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
    }

    private void LoadLevel()
    {
        gridManager.SetLevelData(levelConfig.LevelCSV);
        gridManager.InitializeGrid();
        
        puzzleQuestManager.SetPuzzleQuestData(levelConfig.PuzzleQuestData);
        puzzleQuestManager.SetFirstState();
    }
    

    public void SetLevel(int levelIndex)
    {
        this.currentLevel = levelIndex;
    }

    private bool IsLevelUnlock(int i)
    {
        return i >= currentLevel;
    }
}

