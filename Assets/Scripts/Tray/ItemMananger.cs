using System.Collections.Generic;
using UnityEngine;

public class ItemMananger : MonoBehaviour
{
    public static ItemMananger Instance;
    public int spawnCount = 3;
    public bool isRandom;
    public int maxRandomCount = 0;
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