using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;
    public AnimationConfig AnimationConfig;
    private void Awake()
    {
        Instance = this;
    }

    public void TransferItem(Transform MoveTransform, Vector3 position)
    {
        MoveTo(MoveTransform, position, AnimationConfig.itemTransferDuration);
    }

    public void MoveTrayToSlot(Transform MoveTransform, Vector3 position)
    {
        MoveTo(MoveTransform, position, AnimationConfig.releaseTrayDuration);
    }

    private void MoveTo(Transform MoveTransform, Vector3 position, float duration)
    {
        LMotion.Create(MoveTransform.position, position, duration)
            .BindToPosition(MoveTransform);
    }
}