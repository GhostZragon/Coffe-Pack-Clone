using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Data", menuName = "Audio/Audio Data")]
public class AudioData : ScriptableObject
{
    [SerializeField] private SoundConfig[] soundConfigs;
    public SoundConfig[] SoundConfigs { get => soundConfigs; }
    private void Reset()
    {
        soundConfigs = Resources.LoadAll<SoundConfig>("Sound Asset");
    }
}
