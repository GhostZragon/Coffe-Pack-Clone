using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public abstract class SlotBase : MonoBehaviour
{
    [SerializeField] protected Transform model;
    public abstract bool CanPlacedTray();
    public abstract void ActiveSpecialEffect();

    public abstract void PlayClearAnimation();

    public virtual void InitEffect(float delay)
    {
        Vector3 newPosition = model.localPosition;
        float height = model.localPosition.y + 40;
        newPosition.y = height;

        LMotion.Create(newPosition, Vector3.zero, AnimationManager.Instance.AnimationConfig.normalSlotDropTime)
            .WithEase(Ease.InCubic)
            .WithDelay(delay)
            .BindToLocalPosition(model);
    }
}