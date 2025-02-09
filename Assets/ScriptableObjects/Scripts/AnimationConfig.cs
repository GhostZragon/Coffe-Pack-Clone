using LitMotion;
using UnityEngine;
[CreateAssetMenu(fileName = "Anm_Cfg_",menuName = "SO/ Animation Config ")]
public partial class AnimationConfig : ScriptableObject
{
    public ItemConfig itemcfg;
    public TrayConfig trayCfg;
    public SlotConfig slotCfg;
    public GridConfig gridCfg;
    public LevelUIConfig levelUIConfig;
    public TopUIConfig topUIConfig;
}