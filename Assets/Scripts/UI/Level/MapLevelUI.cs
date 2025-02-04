using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelUI : MonoBehaviour
{
    [SerializeField] private Image MapImage;

    [SerializeField] private int mapIndex;

    private LevelUI[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<LevelUI>();
        TurnOffAllButton();
    }

    [Button]
    public void RefreshMap(ref int startLevel,int endLevel, Predicate<int> isLevelUnlock)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i > endLevel)
            {
                buttons[i].gameObject.SetActive(false);
            }
            else
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].Setup(startLevel++, isLevelUnlock(startLevel));
            }
        }
    }

    private void TurnOffAllButton()
    {
        foreach (var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
    }
}