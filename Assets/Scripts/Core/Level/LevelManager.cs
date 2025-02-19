using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static bool CanMerge => Instance.allowMerge;

    [SerializeField] private bool startQuestByButton = false;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [Header("Gameplay")]
   
    [SerializeField] private bool allowMerge = true;
    [SerializeField] private bool isLoose = false;
    [SerializeField] private bool isWin = false;

    public Action OnWinGame;
    public Action OnLooseGame;
    
    private PuzzleQuestManager puzzleQuestManager;
    private GridManager gridManager;
    private DragDropSystem dragDropSystem;
    private TrayManager trayManager;
    private LevelSelection levelSelection;
    private CollectorManager collectorManager;

    private ItemMananger itemMananger;

    private void Awake()
    {
        Instance = this;
        
        currentLevel = 0;
        
        CatchedRef();

    }
    


    private void CatchedRef()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        puzzleQuestManager = FindFirstObjectByType<PuzzleQuestManager>();
        dragDropSystem = FindFirstObjectByType<DragDropSystem>();
        trayManager = FindFirstObjectByType<TrayManager>();
        levelSelection = FindFirstObjectByType<LevelSelection>();
        collectorManager = FindFirstObjectByType<CollectorManager>();
        itemMananger = FindFirstObjectByType<ItemMananger>();
    }

 
    

    public void LoadLevel()
    {
        var levelConfig = levelSelection.GetCurrentLevelConfig();
        
        gridManager.SetLevelData(levelConfig.LevelCSV);
        gridManager.InitializeGrid();

        itemMananger.SetMaxItemPerTray(trayManager.maxCountPerTray);
        itemMananger.SetFindEmptySlotCallback(GetEmptySlotCount);
        itemMananger.SetCheckingItemOnQuestingCallback(IsItemOnQuesting);
        
        puzzleQuestManager.SetPuzzleQuestData(levelConfig.PuzzleQuestData);
        puzzleQuestManager.SetFirstState();
        puzzleQuestManager.CreateQuests();
        
        // questStageUI.SetMaxStage();
        trayManager.Initialize();
        
    }

    private int GetEmptySlotCount()
    {
        return gridManager.GetEmptySlotCount();
    }

    private bool IsItemOnQuesting(string itemID)
    {
        return puzzleQuestManager.IsItemOnQuesting(itemID);
    }

    public void UnLoadLevel()
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
    }

    [Button]
    private void CheckingWinLosseCondition()
    {
        OnProcessComplete();
    }
    
    public void OnProcessComplete()
    {
        if (puzzleQuestManager.IsFinishAllQuestCurrentStage() && puzzleQuestManager.IsFinalStage() || isWin)
        {
            Debug.Log("You Win");
            OnWinGame();
            return;
        }

        if (gridManager.IsFullOfSpace() || isLoose)
        {
            Debug.Log("You loose");
            OnLooseGame();
            return;
        }

        // check win loose
        trayManager.TryCreateNextTrays();
    }

   
}