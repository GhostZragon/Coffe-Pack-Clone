using System.Collections.Generic;
using UnityEngine;

public class PuzzleQuestManagerUI : MonoBehaviour
{
    public PuzzleQuestUI puzzleQuestUIPrefab;
    public GameObject container;

    private void Awake()
    {
        puzzleQuestUIPrefab.gameObject.SetActive(false);
    }

    public void Bind(InGameQuestData inGameQuestData)
    {
        var questUI = Instantiate(puzzleQuestUIPrefab, container.transform);
   
        questUI.BindingUI(inGameQuestData);
        questUI.gameObject.SetActive(true);
        questUI.OnInitEffect();
    
        Debug.Log("Create item quest UI", questUI.gameObject);
    }
}