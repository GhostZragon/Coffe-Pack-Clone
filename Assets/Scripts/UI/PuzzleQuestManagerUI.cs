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

    public void GetPuzzleQuestUI(PuzzleQuest puzzleQuest)
    {
        var questUI = Instantiate(puzzleQuestUIPrefab, container.transform);
        questUI.BindingUI(puzzleQuest);
        questUI.gameObject.SetActive(true);
        Debug.Log("Create item quest UI", questUI.gameObject);
    }
}