public class BlockingSlot : SlotBase
{
    private bool canDestroy = false;
    public override bool CanPlacedTray()
    {
        return false;
    }

    public override void ActiveSpecialEffect()
    {
        // create new slot at this position
        Table.Instance.ReplaceSlot(this,SlotFactory.Instance.GetSlot(SlotType.Normal));
        canDestroy = true;
        PlayClearAnimation();
    }

    public override void PlayClearAnimation()
    {
        if (canDestroy)
        {
            Destroy(gameObject);
        }
    }
}