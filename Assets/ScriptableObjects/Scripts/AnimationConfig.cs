using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class AnimationConfig : ScriptableObject
{
    public float itemTransferDuration;
    public Ease itemTransferEase;

    public float destroyTrayDuration;
    public Ease destroyTrayEase;
    
}

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
        LMotion.Create(MoveTransform.position, position, AnimationConfig.itemTransferDuration)
            .BindToPosition(MoveTransform);
    }
    
}