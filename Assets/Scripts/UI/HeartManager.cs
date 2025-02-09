using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HeartManager : MonoBehaviour, IEnter
{
    [SerializeField] private int maxCount;
    [SerializeField] private HeartUI HeartPrefab;
    [SerializeField] private GameObject container;

    private HeartUI[] hearts;

    private void Awake()
    {
        hearts = GetComponentsInChildren<HeartUI>();
    }

    private void Start()
    {
        Enter();
    }
    
    [Button]
    public void Enter()
    {
        foreach (var item in hearts)
        {
            item.DropDown();
        }
    }
}
