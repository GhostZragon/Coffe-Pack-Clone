using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelUI : MonoBehaviour
{
    [SerializeField] private Image MapImage;

    [SerializeField] private int mapIndex;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LevelUI[] levelUIs;
  
    [SerializeField] private int startLevel;
    [SerializeField] private int endLevel;
    
    
    public Transform[] GetAllSpawnPoints()
    {
        return spawnPoints;
    }
   
    [Button]
    public void ActiveLevelInMap(ref int startLevel,int endLevel, Predicate<int> isLevelUnlock)
    {
        this.startLevel = startLevel;
        
        for (int i = 0; i < levelUIs.Length; i++)
        {
            if (i > endLevel)
            {
                levelUIs[i].gameObject.SetActive(false);
            }
            else
            {
                levelUIs[i].gameObject.SetActive(true);
                levelUIs[i].Init(startLevel, isLevelUnlock(startLevel));
                startLevel++;
            }
        }

        this.endLevel = startLevel;
    }

    private bool IsLevelInRange(int level)
    {
        // make sure level is in range of map, quickly exit the loop
        return level >= startLevel && level <= endLevel;
    }
    
    
    public bool TryGetLevelUI(int level,out LevelUI levelUI)
    {
        // if this map contain that level, then get it out
        levelUI = null;
        var levelIndex = level - startLevel;
        
        if (IsLevelInRange(level) && levelIndex >= 0 && levelIndex < levelUIs.Length)
        {
            levelUI = levelUIs[levelIndex];
        }

        return levelUI != null;
    }
    
    private void TurnOffAllButton()
    {
        foreach (var item in levelUIs)
        {
            item.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Assign references and turn off all UI
    /// </summary>
    /// <param name="_levelUis"></param>
    public void InitLevelUIs(LevelUI levelUIPrefab)
    {
        levelUIs = new LevelUI[spawnPoints.Length];

        for (int i = 0; i < levelUIs.Length; i++)
        {
            levelUIs[i] = Instantiate(levelUIPrefab, spawnPoints[i].position, Quaternion.identity,transform);
        }
        
        TurnOffAllButton();
    }
}