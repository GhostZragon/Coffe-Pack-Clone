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
    
    public float clearTrayDelay = 0.2f;

    public float normalSlotDropTime = 0.5f;
    
    public float rowDelayFactor = 0.1f; // Thời gian delay giữa các hàng
    public float columnDelayFactor = 0.05f; // Thời gian delay giữa các cột
}