using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class BlockingSlot : SlotBase
{
    private bool canDestroy = false;

    public override bool CanPlacedTray()
    {
        return false;
    }

    public override void ActiveSpecialEffect()
    {
        TestEffect();
    }

    [Button]
    public void TestEffect()
    {
        LMotion.Punch.Create(model.transform.position, new Vector3(-0.2f, 0.2f, 0), 0.4f)
            .WithDampingRatio(1)
            .WithEase(Ease.InCubic)
            .WithFrequency(15)
            .WithOnComplete(Effect)
            .BindToPosition(model.transform);
    }

    private void Effect()
    {
        EventManager.Current._Table.OnReplaceSlot(this, SlotManager.Instance.GetSlot(SlotType.Normal));
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