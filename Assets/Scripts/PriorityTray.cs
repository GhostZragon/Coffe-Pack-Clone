using System;

[Serializable]
public class PriorityTray: IComparable<PriorityTray>
{
    public Tray Tray;
    public string MainItemID;
    public int MainLevel;
    public int SubLevel;
    public bool isPlacedSlot = false;
    private TrayDebugVisualize trayDebugVisualize;
    public void RefreshDebugView()
    {
        if (trayDebugVisualize == null)
        {
            trayDebugVisualize = Tray.GetComponent<TrayDebugVisualize>();
        }
        trayDebugVisualize.Refresh(this);
    }
    
    public void Clear()
    {
        if (trayDebugVisualize == null)
        {
            trayDebugVisualize = Tray.GetComponent<TrayDebugVisualize>();
        }    
        trayDebugVisualize.Refresh(null);
    }

    public void Init(Tray checkingTray, string itemID, bool isCheckingSlot = false)
    {
        Tray = checkingTray;
        MainItemID = itemID;
        isPlacedSlot = isCheckingSlot;
        Calculator();
    }

    public void Calculator()
    {
        int uniqueItemCount = Tray.GetUniqueItemIDs().Count;
        int itemCount = Tray.GetCountOfItem(MainItemID);

        // If the tray is full, add 0; otherwise, add 1
        int trayFullBonus = Tray.CanAddMoreItem() ? 0 : 1;

        // If this slot was just placed by the player, add 1
        int playerPlacedSlotBonus = isPlacedSlot ? 5 : 0;
        
        MainLevel = uniqueItemCount;
        SubLevel = itemCount + trayFullBonus + playerPlacedSlotBonus;
    }
    
    public int CompareTo(PriorityTray other)
    {
        if (other == null) return 1;
        
        
        // So sánh MainLevel trước (tăng dần)
        int mainLevelComparison = this.MainLevel.CompareTo(other.MainLevel);
        
        // Nếu MainLevel bằng nhau thì so sánh SubLevel (tăng dần)
        if (mainLevelComparison == 0)
        {
            return this.SubLevel.CompareTo(other.SubLevel);
        }
        
        return mainLevelComparison;
    }
}