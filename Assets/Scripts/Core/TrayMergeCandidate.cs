using System;

[Serializable]
public class TrayMergeCandidate: IComparable<TrayMergeCandidate>
{
    public Tray Tray;
    public string ItemId;
    public int MainLevel;
    public int SubLevel;
    public bool isOriginTray = false;
 
    public void Initialize(Tray checkingTray, string itemID, bool isCheckingSlot = false)
    {
        Tray = checkingTray;
        ItemId = itemID;
        isOriginTray = isCheckingSlot;
        RecalculatePriority();
    }

    public void RecalculatePriority()
    {
        int uniqueItemCount = Tray.GetUniqueItemIDs().Count;
        int itemCount = Tray.GetCountOfItem(ItemId);

        // If the tray is full, add 0; otherwise, add 1
        int trayFullBonus = Tray.CanAddMoreItem() ? 0 : 1;

        // If this slot was just placed by the player, add 1
        int playerPlacedSlotBonus = isOriginTray ? 5 : 0;
        
        MainLevel = uniqueItemCount;
        SubLevel = itemCount + trayFullBonus + playerPlacedSlotBonus;
    }
    
    public int CompareTo(TrayMergeCandidate other)
    {
        if (other == null) return 1;
        
        
        // So sánh MainLevel trước (tăng dần)
        int mainLevelComparison = this.MainLevel.CompareTo(other.MainLevel);
        
        // Nếu MainLevel bằng nhau thì so sánh SubLevel (tăng dần)
        if (mainLevelComparison == 0)
        {
            // Nếu chỉ có item và không phải là slot người chơi đặt xuống, thì ưu tiên khay có nhiều item hơn
            if(MainLevel == 1 && other.isOriginTray == false && isOriginTray == false)
                return -this.SubLevel.CompareTo(other.SubLevel);

            return this.SubLevel.CompareTo(other.SubLevel);
        }
        
        return mainLevelComparison;
    }
}