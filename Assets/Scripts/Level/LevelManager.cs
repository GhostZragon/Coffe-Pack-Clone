using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig[] levelConfigs;

    private int levelIndex = 0;
    
    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;

    [SerializeField] private bool startQuestByButton = false;
    private void Awake()
    {
        levelIndex = 0;
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        levelConfigs = Resources.LoadAll<LevelConfig>("Level");
    }

    private void Start()
    {
        if (levelConfig == null)
        {
            levelConfig = levelConfigs[levelIndex];
        }

        if (startQuestByButton == false)
        {
            LoadLevel();
        }
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
        this.levelIndex = levelIndex;
    }
}
