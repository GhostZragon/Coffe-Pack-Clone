using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private LevelPanelUI levelPanelUI;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel = 0;

    private void Awake()
    {
        levelConfigs = Resources.LoadAll<LevelConfig>("Level");
        Register();
    }

    private void OnDestroy()
    {
        UnRegister();
    }

    private void Start()
    {
        if (levelConfig == null)
        {
            levelConfig = levelConfigs[currentLevel];
        }

        SettingsLevel();
    }

    private void Register()
    {
        EventManager.Current._Core.OnSelectLevel += SetLevel;
        levelPanelUI.levelUnlockChecking = IsLevelUnlock;

    }

    private void UnRegister()
    {
        EventManager.Current._Core.OnSelectLevel -= SetLevel;
    }
    
    private void SetLevel(int levelIndex)
    {
        this.currentLevel = levelIndex;
        levelConfig = levelConfigs[currentLevel];
    }
    
    private void SettingsLevel()
    {
        // TODO: Split level map UI logic creator to another class
        maxLevel = levelConfigs.Length - 1;
        currentLevel = Mathf.Clamp(currentLevel, 0, levelConfigs.Length);

        levelPanelUI.Initialize(maxLevel);

        EventManager.Current._Core.OnSelectLevel?.Invoke(currentLevel);
    }
    
    private bool IsLevelUnlock(int i)
    {
        return true;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return levelConfig;
    }
}