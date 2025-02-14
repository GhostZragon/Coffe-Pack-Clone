using System;
using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public static TrayManager instance;
    [SerializeField] private GameObject trayContainer;
    [SerializeField] private List<Transform> trayStandPositions;
    [SerializeField] private List<Tray> currentTrayList = new();
    [SerializeField] private int maxCountPerTray = 6;
    [SerializeField] private bool isUsingTestCaseSO = false;

  
    
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
            var tray = Instantiate(trayPrefab, trayStandPositions[i].position, Quaternion.identity,trayContainer.transform);
            tray.Index = i;
            tray.name = "Tray_" + totalCount;
            tray.SetMaxCount(maxCountPerTray);
            tray.RequestItem();
            // tray.RequestItem();
            RegisterTray(tray);
            totalCount++;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAllTrays();
            TryCreateNextTrays();
        }
    }

    public void ClearAllTrays()
    {
        foreach (var item in currentTrayList)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in trayContainer.transform)
        {
            Destroy(item.gameObject);
        }
        currentTrayList.Clear();
        totalCount = 0; 

    }

    public Transform GetStandPosition(int index)
    {
        if (index >= 0 && index < trayStandPositions.Count)
        {
            return trayStandPositions[index];
        }
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
    }

    public void TryCreateNextTrays()
    {
        if (currentTrayList.Count == 0)
        {
            Initialize();
        }
    }

    public void RegisterTray(Tray tray)
    {
        currentTrayList.Add(tray);
    }

    public void Remove(Tray tray)
    {
        currentTrayList.Remove(tray);
    }
    
}

