using Object = UnityEngine.Object;

public class GameplayState : StateWithSubStates
{

    private PuzzleQuestManager puzzleQuestManager;
    private DragDropSystem dragDropSystem;
    private LevelManager levelManager;
    private ResultData resultData;
    private Table table;
    // UI
    private PuzzleQuestManagerUI PuzzleQuestManagerUI;
    private QuestStageUI questStageUI;
    public override void PrepareState()
    {
        base.PrepareState();
        levelManager.LoadLevel();
        UIManager.Instance.ShowGameplayUI();
        
        ChangeSubState<NormalPlayingState>();
    }

    protected override void CatchRef()
    {
        levelManager = LevelManager.Instance;
        puzzleQuestManager = levelManager.puzzleQuestManager;
        table = levelManager.table;
        dragDropSystem = levelManager.dragDropSystem;
        
        questStageUI = UIManager.Instance.gameplayUI.QuestStageUI;
        PuzzleQuestManagerUI = UIManager.Instance.gameplayUI.PuzzleQuestManagerUI;
    }

    protected override void Register()
    {
        base.Register();
        
        RegisterSubState(new NormalPlayingState(this));
        RegisterSubState(new PauseGameState(this));

        Slot.OnMergeSlotAction += MergeSlot;
        BlockingSlot.OnReplaceSlotAction += OnReplaceSlot;
        Slot.OnDestroyBlockingBlockAroundAction += OnDestroyBlockingBlockAround;
        Slot.OnCompleteItemAction += OnCompleteItem;
        
        table.OnProcressComplete += OnProcessComplete;
        levelManager.OnWinGame += WinGame;
        levelManager.OnLooseGame += LooseGame;
        
        puzzleQuestManager.OnBindingQuestToUIAction += BidingQuestWithUI;
        puzzleQuestManager.OnChangedStage += questStageUI.OnStageChanged;

        UIManager.Instance.gameplayUI.OpenPauseMenuClicked += Pause;

    }

    public override void DestroyState()
    {
        // static action
        base.DestroyState();
        
        levelManager.UnLoadLevel();
        questStageUI.ResetProgressUI();
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        
        Slot.OnMergeSlotAction -= MergeSlot;
        BlockingSlot.OnReplaceSlotAction -= OnReplaceSlot;
        Slot.OnDestroyBlockingBlockAroundAction -= OnDestroyBlockingBlockAround;
        Slot.OnCompleteItemAction -= OnCompleteItem;

        puzzleQuestManager.OnBindingQuestToUIAction -= BidingQuestWithUI;
        puzzleQuestManager.OnChangedStage -= questStageUI.OnStageChanged;

        
        table.OnProcressComplete -= OnProcessComplete;
        levelManager.OnWinGame -= WinGame;
        levelManager.OnLooseGame -= LooseGame;
        
        UIManager.Instance.gameplayUI.OpenPauseMenuClicked -= Pause;
    }

    private void OnCompleteItem(ItemInfo itemInfo)
    {
        puzzleQuestManager.OnCompleteItem(itemInfo);
    }
    
    private void OnDestroyBlockingBlockAround(SlotBase slotBase)
    {
        table.DestroyBlockingSlotAround(slotBase);
    }

    private void OnReplaceSlot(SlotBase slot1, SlotBase slot2)
    {
        table.ReplaceSlot(slot1,slot2);
    }

    private void MergeSlot(Slot slot)
    {
        table.mergeSystem.TryMergeAtSlot(slot);
    }
    

    private void BidingQuestWithUI(InGameQuestData inGameQuestData)
    {
        PuzzleQuestManagerUI.Bind(inGameQuestData);
    }


    private void OnProcessComplete()
    {
        levelManager.OnProcessComplete();
    }
    
    private void LooseGame()
    {
        // init resultData
        ChangeGameResultState();
    }

    private void WinGame()
    {
        // init resultData
        ChangeGameResultState();
    }

    private void ChangeGameResultState()
    {
        ChangeState(new GameResultState(resultData));
    }
    private void BackMenu()
    {
        ChangeState(new MainMenuState());
    }

    private void Resume()
    {       
        ChangeSubState<NormalPlayingState>();
    }

    private void Pause()
    {
        ChangeSubState<PauseGameState>();
    }
    
    private class NormalPlayingState : ISubState
    {
        private GameplayState gameplayState;
        public NormalPlayingState(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }
        public void Enter()
        {
            gameplayState.dragDropSystem.SetDragging(true);
        }

        public void Exit()
        {
            gameplayState.dragDropSystem.SetDragging(false);
        }
    }
    private class PauseGameState : ISubState
    {
        private GameplayState gameplayState;
        private PauseUI pauseUI;
        public PauseGameState(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }
        public void Enter()
        {
            // Show Pause UI
            pauseUI = UIManager.Instance.pauseUI;
            pauseUI.Show();
            
            pauseUI.OnResumeClicked += gameplayState.Resume;
            pauseUI.OnBackMainMenuClicked += gameplayState.BackMenu;
        }

        public void Exit()
        {
            pauseUI.OnResumeClicked += gameplayState.Resume;
            pauseUI.OnBackMainMenuClicked -= gameplayState.BackMenu;
            pauseUI.Hide();
            // Hide Pause UI 
        }
    }
    
}
// tutorial state
// pause game state