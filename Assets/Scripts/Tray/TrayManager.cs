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
        CreateTrays();
    }

    private void CreateTrays()
    {
        for (int i = 0; i < trayStandPositions.Count; i++)
        {
            var tray = Instantiate(trayPrefab, trayStandPositions[i].position, Quaternion.identity);
            tray.Index = i;
            tray.RequestItem();
           
            Add(tray);
        }
    }


    public Transform GetStandPosition(int index)
    {
        return trayStandPositions[index];
    }

    public void TryCreateNextTrays()
    {
        if (currentTrayList.Count == 0)
        {
            CreateTrays();
        }
    }

    public void Add(Tray tray)
    {
        currentTrayList.Add(tray);
    }

    public void Remove(Tray tray)
    {
        currentTrayList.Remove(tray);
    }
}