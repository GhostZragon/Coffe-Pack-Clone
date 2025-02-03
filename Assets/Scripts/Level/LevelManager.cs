using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig defaultLevelConfig;

    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;

    [SerializeField] private bool startQuestByButton = false;
    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        defaultLevelConfig = Resources.Load<LevelConfig>("Level/cfg_level 1");
    }

    private void Start()
    {
        if (levelConfig == null)
        {
            levelConfig = defaultLevelConfig;
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

    public void SetLevel(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
    }
}
