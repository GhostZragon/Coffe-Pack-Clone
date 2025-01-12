using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrayManager : MonoBehaviour
{
    public static TrayManager instance;

    [SerializeField] private List<Transform> trayStandPositions;
    [SerializeField] private List<Item> itemsList;
    [SerializeField] private bool isUsingTestCaseSO = false;

    public Tray trayPrefab;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < trayStandPositions.Count; i++)
        {
            var tray = Instantiate(trayPrefab, trayStandPositions[i].position, Quaternion.identity);
            tray.Index = i;
            AddItemToTray(tray);
        }
    }

    private void AddItemToTray(Tray tray)
    {
        var item = Instantiate(itemsList[Random.Range(0, itemsList.Count)]);
        tray.Add(item);
    }

    public Transform GetStandPosition(int index)
    {
        if (index == -1) return null;

        return trayStandPositions[index];
    }
}
