using System.Collections.Generic;
using UnityEngine;

public class PuzzleQuestManagerUI : MonoBehaviour
{
    public PuzzleQuestUI puzzleQuestUIPrefab;
    public GameObject container;

    private void Awake()
    {
        puzzleQuestUIPrefab.gameObject.SetActive(false);
        EventManager.Current._UI.OnBindingWithQuestUI += GetPuzzleQuestUI;
    }

    private void OnDestroy()
    {
        EventManager.Current._UI.OnBindingWithQuestUI -= GetPuzzleQuestUI;
    }

    private void GetPuzzleQuestUI(InGameQuestData inGameQuestData)
    {
        var questUI = Instantiate(puzzleQuestUIPrefab, container.transform);
   
        questUI.BindingUI(inGameQuestData);
        questUI.gameObject.SetActive(true);
    
        Debug.Log("Create item quest UI", questUI.gameObject);
    }
}