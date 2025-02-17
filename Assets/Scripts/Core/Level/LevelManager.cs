using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private bool startQuestByButton = false;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [Header("Gameplay")]
    [SerializeField] private QuestStageUI questStageUI;
    private static LevelManager instance;
    public static bool CanMerge => instance.allowMerge;
    [SerializeField] private bool allowMerge = true;
    [SerializeField] private bool isLoose = false;
    [SerializeField] private bool isWin = false; 
    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;
    private DragDropSystem dragDropSystem;
    private TrayManager trayManager;
    private LevelSelection levelSelection;

    private void Awake()
    {
        instance = this;
        
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
        levelSelection = FindFirstObjectByType<LevelSelection>();
    }

    private void Register()
    {
     
        puzzleQuestManager.OnChangedStage += questStageUI.OnStageChanged;
        puzzleQuestManager.OnSetMaxStage += questStageUI.SetMaxStage;

        EventManager.Current._Core.OnLoadLevel += LoadLevel;
        EventManager.Current._Core.OnUnloadLevel += UnLoadLevel;

        EventManager.Current._Core.OnProcessComplete += OnProcessComplete;
    }

    private void UnRegister()
    {
      
        puzzleQuestManager.OnChangedStage -= questStageUI.OnStageChanged;
        puzzleQuestManager.OnSetMaxStage -= questStageUI.SetMaxStage;

        EventManager.Current._Core.OnLoadLevel -= LoadLevel;
        EventManager.Current._Core.OnUnloadLevel -= UnLoadLevel;

        EventManager.Current._Core.OnProcessComplete -= OnProcessComplete;
    }

    private void Start()
    {
       
        if (startQuestByButton == false)
        {
            LoadLevel();
        }
    }

    

    private void LoadLevel()
    {
        var levelConfig = levelSelection.GetCurrentLevelConfig();
        
        gridManager.SetLevelData(levelConfig.LevelCSV);
        gridManager.InitializeGrid();

        puzzleQuestManager.SetPuzzleQuestData(levelConfig.PuzzleQuestData);
        puzzleQuestManager.SetFirstState();
        puzzleQuestManager.CreateQuests();
        
        // questStageUI.SetMaxStage();
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

    [Button]
    private void CheckingWinLosseCondition()
    {
        OnProcessComplete();
    }
    
    private void OnProcessComplete()
    {
        if (puzzleQuestManager.IsFinishAllQuestCurrentStage() && puzzleQuestManager.IsFinalStage() || isWin)
        {
            Debug.Log("You Win");
            return;
        }

        if (gridManager.IsFullOfSpace() || isLoose)
        {
            Debug.Log("You loose");
            return;
        }

        // check win loose
        trayManager.TryCreateNextTrays();
    }

}