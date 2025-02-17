using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static bool CanMerge => instance.allowMerge;

    [SerializeField] private bool startQuestByButton = false;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [Header("Gameplay")]
    [SerializeField] private QuestStageUI questStageUI;
   
    [SerializeField] private bool allowMerge = true;
    [SerializeField] private bool isLoose = false;
    [SerializeField] private bool isWin = false; 
   
    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;
    private DragDropSystem dragDropSystem;
    private TrayManager trayManager;
    private LevelSelection levelSelection;
    private CollectorManager collectorManager;

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

        EventManager.Current._Core.OnLoadLevel += LoadLevel;
        EventManager.Current._Core.OnUnloadLevel += UnLoadLevel;
        EventManager.Current._Core.OnReloadGame += ReloadLevel;

        EventManager.Current._Core.OnProcessComplete += OnProcessComplete;
    }

    private void UnRegister()
    {
      
        puzzleQuestManager.OnChangedStage -= questStageUI.OnStageChanged;

        EventManager.Current._Core.OnLoadLevel -= LoadLevel;
        EventManager.Current._Core.OnUnloadLevel -= UnLoadLevel;
        EventManager.Current._Core.OnReloadGame -= ReloadLevel;

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
        // don slot tren scene
        gridManager.ClearGrid();
        // don ui tren scene
        puzzleQuestManager.ClearQuest();
        // don tray dang co
        trayManager.ClearAllTrays();
        // don item drag neu co
        dragDropSystem.ClearDragItem();
        // khoi dong lai UI
        questStageUI.ResetProgressUI();
        

    }

    private void ReloadLevel()
    {
        UnLoadLevel();
        LoadLevel();
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
            ShowResult();
            return;
        }

        if (gridManager.IsFullOfSpace() || isLoose)
        {
            Debug.Log("You loose");
            ShowResult();
            return;
        }

        // check win loose
        trayManager.TryCreateNextTrays();
    }

    private void ShowResult()
    {
        EventManager.Current._UI.OnShowResultUI?.Invoke(new ResultData(puzzleQuestManager.GetCurrentStage(),0));
        UnLoadLevel();
    }
}