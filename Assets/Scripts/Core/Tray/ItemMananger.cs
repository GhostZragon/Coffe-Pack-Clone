using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-99)]
public class ItemMananger : MonoBehaviour
{
    public static ItemMananger Instance;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private bool isRandom;
    [SerializeField] private int maxRandomCount = 0;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private List<Item> itemsList;

    public Item GetNewItem()
    {
        var item = Instantiate(itemsList[Random.Range(0, itemsList.Count)]);
        return item;
    }
}