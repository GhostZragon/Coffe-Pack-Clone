using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameplayState : StateWithSubStates
{

    private PuzzleQuestManager puzzleQuestManager;
    private DragDropSystem dragDropSystem;
    private LevelManager levelManager;
    private Table table;
    // UI
    private PuzzleQuestManagerUI PuzzleQuestManagerUI;
    private QuestStageUI questStageUI;

    private GameSessionController gameSessionController;

    protected override void AfterPrepareState()
    {
        base.AfterPrepareState();
        
        levelManager.LoadLevel();
        UIManager.Instance.ShowGameplayUI();
        ChangeSubState<NormalPlayingState>();
        gameSessionController = DataManager.Instance.GetDataController<GameSessionController>();
        gameSessionController.ResetSession();
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
        RegisterSubState(new EndGameplayState(this));

        Slot.OnMergeSlotAction += MergeSlot;
        BlockingSlot.OnReplaceSlotAction += OnReplaceSlot;
        Slot.OnDestroyBlockingBlockAroundAction += OnDestroyBlockingBlockAround;
        Slot.OnCompleteItemAction += OnCompleteItem;
        
        table.OnProcressComplete += OnProcessComplete;
        levelManager.OnWinGame += WinGame;
        levelManager.OnLooseGame += LooseGame;
        
        puzzleQuestManager.OnBindingQuestToUIAction += BidingQuestWithUI;
        //puzzleQuestManager.OnChangedStage += questStageUI.OnStageChanged;

        UIManager.Instance.gameplayUI.OpenPauseMenuClicked += Pause;

    }

    protected override void AfterDestroyState()
    {
        base.AfterDestroyState();
        ResetToDefaultState();
    }

    private void ResetToDefaultState()
    {
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
        //puzzleQuestManager.OnChangedStage -= questStageUI.OnStageChanged;

        
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
        gameSessionController.SetGameResult(GameResult.Lose);

        EndGame();
    }

    private void WinGame()
    {
        // init resultData
        gameSessionController.SetGameResult(GameResult.Win);
        EndGame();
    }

    private void EndGame()
    {
        //ChangeState(new GameResultState());
        ChangeSubState<EndGameplayState>();
    }

    private void BackMenu()
    {
        ChangeState(new MainMenuState());
    }

    private void Resume()
    {       
        ChangeSubState<NormalPlayingState>();
    }

    private void ChangeToResultState()
    {
        ChangeState(new GameResultState());
    }

    private void Pause()
    {
        ChangeSubState<PauseGameState>();
    }

    private void ResetGameplay()
    {
        ResetToDefaultState();
        levelManager.LoadLevel();
        ChangeSubState<NormalPlayingState>();
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
        private readonly GameplayState gameplayState;
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
            pauseUI.OnResetButtonClicked += gameplayState.ResetGameplay;
        }

        public void Exit()
        {
            pauseUI.OnResumeClicked += gameplayState.Resume;
            pauseUI.OnBackMainMenuClicked -= gameplayState.BackMenu;
            pauseUI.OnResetButtonClicked -= gameplayState.ResetGameplay;
           
            pauseUI.Hide();
            // Hide Pause UI 
        }
    }

    public class EndGameplayState : ISubState
    {
        private readonly GameplayState gameplayState;
        public EndGameplayState(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }
        public void Enter()
        {
            // chay animation add coin
            // cứ mỗi star active là + 5 coin
            // nếu win thì cộng + 10 coin
            var data = gameplayState.gameSessionController.Data;
            int rewardCoin = data.StarUnlocked * 5;
            int winLevelCoin = data.GameResult == GameResult.Win ? 10 : 0;

            gameplayState.gameSessionController.AddRewardCoin(rewardCoin + winLevelCoin);
            gameplayState.levelManager.StartCoroutine(WaitOneSecond());

            // using some effect for clearing table 
        }

        private IEnumerator WaitOneSecond()
        {
            Debug.Log("================");

            Debug.Log("Wait for one second");
            yield return new WaitForSeconds(1);
            gameplayState.ChangeToResultState();
        }

        public void Exit()
        {
        }
    }
}
// tutorial state
// pause game state
