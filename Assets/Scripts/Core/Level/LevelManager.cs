using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private LevelPanelUI levelPanelUI;
    
    [SerializeField] private bool startQuestByButton = false;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [Header("Gameplay")]
    [SerializeField] private QuestStageUI questStageUI;



    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;
    private DragDropSystem dragDropSystem;
    private TrayManager trayManager;

    private void Awake()
    {
        currentLevel = 0;
        
        CatchedRef();

        Register();
    }
    
    private void OnDestroy()
    {
        UnRegister();
    }

    private void CatchedRef()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        dragDropSystem = FindFirstObjectByType<DragDropSystem>();
        trayManager = FindFirstObjectByType<TrayManager>();

        levelConfigs = Resources.LoadAll<LevelConfig>("Level");
    }

    private void Register()
    {
        levelPanelUI.levelUnlockChecking = IsLevelUnlock;
     
        puzzleQuestManager.OnChangedStage += questStageUI.OnStageChanged;
        
        EventManager.Current._Core.OnLoadLevel += LoadLevel;
        EventManager.Current._Core.OnUnloadLevel += UnLoadLevel;
        EventManager.Current._Core.OnSelectLevel += SetLevel;

        EventManager.Current._Core.OnProcessComplete += OnProcessComplete;
    }

    private void UnRegister()
    {
        levelPanelUI.levelUnlockChecking = null;
      
        puzzleQuestManager.OnChangedStage -= questStageUI.OnStageChanged;

        EventManager.Current._Core.OnLoadLevel -= LoadLevel;
        EventManager.Current._Core.OnUnloadLevel -= UnLoadLevel;
        EventManager.Current._Core.OnSelectLevel -= SetLevel;

        EventManager.Current._Core.OnProcessComplete -= OnProcessComplete;
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
        // TODO: Split level map UI logic creator to another class
        maxLevel = levelConfigs.Length - 1;
        currentLevel = Mathf.Clamp(currentLevel, 0, levelConfigs.Length);

        levelPanelUI.Init(maxLevel);

        EventManager.Current._Core.OnSelectLevel?.Invoke(currentLevel);
    }

    private void LoadLevel()
    {
        gridManager.SetLevelData(levelConfig.LevelCSV);
        gridManager.InitializeGrid();

        puzzleQuestManager.SetPuzzleQuestData(levelConfig.PuzzleQuestData);
        puzzleQuestManager.SetFirstState();
        puzzleQuestManager.CreateQuests();

        questStageUI.SetMaxStage(puzzleQuestManager.GetMaxStage());
        trayManager.Initialize();
        
        UIManager.Instance.ShowGameplayUI();
    }


    private void UnLoadLevel()
    {
        gridManager.ClearGrid();
        puzzleQuestManager.ClearQuest();
        trayManager.ClearAllTrays();
        dragDropSystem.ClearDragItem();
        questStageUI.ResetUI();
    }

    private void SetLevel(int levelIndex)
    {
        this.currentLevel = levelIndex;
        levelConfig = levelConfigs[currentLevel];
    }

    private void OnProcessComplete()
    {
        if (puzzleQuestManager.IsRunOutOfQuest())
        {
            // 
            Debug.Log("You Win");
            return;
        }

        if (gridManager.IsFullOfSpace())
        {
            Debug.Log("You loose");
            return;
        }

        // check win loose
        trayManager.TryCreateNextTrays();
    }

    private bool IsLevelUnlock(int i)
    {
        return i <= currentLevel;
    }
}

