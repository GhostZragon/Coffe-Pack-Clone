using LitMotion;
using UnityEngine;
[CreateAssetMenu(fileName = "Anm_Cfg_",menuName = "SO/ Animation Config ")]
public class AnimationConfig : ScriptableObject
{
    public float itemTransferDuration;
    public float itemTransferStartDelay = .1f;
    public Ease itemTransferEase;

    public float releaseTrayDuration;
    public Ease releaseTrayEase;
    
    public float destroyTrayDuration;
    public Ease destroyTrayEase;
}