using System;
using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public static TrayManager instance;

    [SerializeField] private List<Transform> trayStandPositions;
    [SerializeField] private bool isUsingTestCaseSO = false;
    [SerializeField] private List<Tray> currentTrayList = new();
    public Tray trayPrefab;
    private void Awake()
    {
        instance = this;
    }

    private int totalCount = 0;
    public void Initialize()
    {
        for (int i = 0; i < trayStandPositions.Count; i++)
        {
            var tray = Instantiate(trayPrefab, trayStandPositions[i].position, Quaternion.identity);
            tray.Index = i;
            tray.name = "Tray_" + totalCount;
            // tray.RequestItem();
            CatchingTray(tray);
            totalCount++;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAllTray();
            TryCreateNextTrays();
        }
    }

    public void ClearAllTray()
    {
        foreach (var item in currentTrayList)
        {
            Destroy(item.gameObject);
        }

        currentTrayList.Clear();
    }

    public Transform GetStandPosition(int index)
    {
        return trayStandPositions[index];
    }

    public void TryCreateNextTrays()
    {
        if (currentTrayList.Count == 0)
        {
            Initialize();
        }
    }

    public void CatchingTray(Tray tray)
    {
        currentTrayList.Add(tray);
    }

    public void Remove(Tray tray)
    {
        currentTrayList.Remove(tray);
    }
    
}

public interface IGameControl
{
    void Init();
    void Clear();
}