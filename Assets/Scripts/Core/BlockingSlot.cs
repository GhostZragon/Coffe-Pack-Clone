public class BlockingSlot : SlotBase
{
    public override bool CanPlacedTray()
    {
        return false;
    }

    public override void ActiveSpecialEffect()
    {
        // create new slot at this position
        Table.Instance.ReplaceSlot(this,SlotFactory.Instance.GetSlot(SlotType.Normal));
    }
}