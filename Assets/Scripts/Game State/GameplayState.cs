using System;
using Object = UnityEngine.Object;

public class GameplayState : BaseState
{

    private PuzzleQuestManager puzzleQuestManager;
    private LevelManager levelManager;
    private ResultData resultData;
    private Table table;
    // UI

    private QuestStageUI questStageUI;
    public override void PrepareState()
    {
        base.PrepareState();
        levelManager.LoadLevel();
        UIManager.Instance.ShowGameplayUI();
    }

    protected override void CatchRef()
    {
        levelManager = LevelManager.Instance;
        puzzleQuestManager = Object.FindFirstObjectByType<PuzzleQuestManager>();
        table = Object.FindFirstObjectByType<Table>();
        questStageUI = UIManager.Instance.gameplayUI.QuestStageUI;
    }

    protected override void Register()
    {
        base.Register();
        Slot.OnMergeSlotAction += MergeSlot;
        BlockingSlot.OnReplaceSlotAction += OnReplaceSlot;
        Slot.OnDestroyBlockingBlockAroundAction += OnDestroyBlockingBlockAround;
        Slot.OnCompleteItemAction += OnCompleteItem;
        
        table.OnProcressComplete += OnProcessComplete;
        levelManager.OnWinGame += WinGame;
        levelManager.OnLooseGame += LooseGame;
        
        puzzleQuestManager.OnBindingQuestToUIAction += BidingQuestWithUI;
        puzzleQuestManager.OnChangedStage += questStageUI.OnStageChanged;
        
        UIManager.Instance.gameplayUI.BackMenuButtonClicked += BackMenuButtonClicked;
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

        UIManager.Instance.gameplayUI.BackMenuButtonClicked -= BackMenuButtonClicked;
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
        UIManager.Instance.gameplayUI.PuzzleQuestManagerUI.Bind(inGameQuestData);
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
    private void BackMenuButtonClicked()
    {
        ChangeState(new MainMenuState());
    }

}
// tutorial state
// pause game state
// 
