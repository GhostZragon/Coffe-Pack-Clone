using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig[] levelConfigs;
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }

    public int UnlockedLevel;

    
    public void SetLevel(int levelIndex)
    {
        this.CurrentLevel = levelIndex;
        levelConfig = levelConfigs[CurrentLevel];
    }
    
    public void SettingsLevel()
    {
        levelConfigs = Resources.LoadAll<LevelConfig>("Level");

        // TODO: Split level map UI logic creator to another class
        MaxLevel = levelConfigs.Length - 1;
        CurrentLevel = Mathf.Clamp(CurrentLevel, 0, levelConfigs.Length);

    }
    
    public bool IsLevelUnlock(int level)
    {
        return level <= UnlockedLevel;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return levelConfig;
    }

    
}