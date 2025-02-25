using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DefaultExecutionOrder(-99)]
public class ItemMananger : MonoBehaviour
{
    public static ItemMananger Instance;
    
    [SerializeField] private int spawnCount = 3;
    
    [SerializeField] private List<Item> itemsList;
    private Dictionary<string, float> spawnWeights = new Dictionary<string, float>(); // Trọng số random
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // Số lượng từng item trên bàn

    private void Awake()
    {
        Instance = this;
        foreach (var item in itemsList)
        {
            itemCounts[item.itemID] = 0;
        }
    }


    private int totalSlots;
    private int maxSlotPerTray = 4;
  
    private Func<int> GetEmptySlotCountCallback;
    private Predicate<string> IsItemOnQuestingCallback;

  

    public Item GetNewItem()
    {
        var itemPrefabID = GetRandomItem();
        var itemPrefab = GetPrefab(itemPrefabID);
        Debug.Log("Add item id " +itemPrefabID);
        if (itemPrefab == null) return null;
        
        var item = Instantiate(itemPrefab);
        AddItem(item.itemID);
        return item;
    }

    private Item GetPrefab(string itemID)
    {
        foreach (var item in itemsList)
        {
            if (item.itemID == itemID)
                return item;
        }

        return null;
    }

    public string GetRandomItem()
    {
        UpdateSpawnWeights();

        // Chọn item dựa trên trọng số
        float totalWeight = spawnWeights.Values.Sum();
        float randValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0;

        foreach (var item in spawnWeights)
        {
            cumulative += item.Value;
            if (randValue <= cumulative)
                return item.Key;
        }

        return itemsList[0].itemID; // Dự phòng (không bao giờ xảy ra)
    }
    [Header("Difficult Settings")] 
    [SerializeField] private float existingItemWeightMultiplier = 1.5f; // Trọng số khi item đã tồn tại trên bàn
    [SerializeField] private float newItemWeightReductionWhenFull = 0.5f; // Giảm trọng số item mới khi bàn đầy
    [SerializeField] private float questItemWeightMultiplier = 1.5f; // Trọng số khi item cần để hoàn thành nhiệm vụ

    private void UpdateSpawnWeights()
    {
        totalSlots = GetEmptySlotCountCallback.Invoke();
        spawnWeights.Clear();
    
        int totalPlacedItems = itemCounts.Values.Sum();

        foreach (var item in itemsList)
        {
            float weight = 1.0f;

            // Tăng trọng số nếu item đã có trên bàn nhưng chưa đạt giới hạn merge
            if (itemCounts[item.itemID] > 0 && itemCounts[item.itemID] < maxSlotPerTray)
                weight *= existingItemWeightMultiplier;

            // Giảm trọng số spawn item mới nếu bàn gần đầy
            if (totalPlacedItems >= totalSlots * 0.8f && itemCounts[item.itemID] == 0)
                weight *= newItemWeightReductionWhenFull;

            // Tăng trọng số nếu item cần thiết để hoàn thành nhiệm vụ
            if (IsItemOnQuestingCallback(item.itemID))
                weight *= questItemWeightMultiplier;

            spawnWeights[item.itemID] = weight;
        }
    }

    // private bool NeedMoreOf(string item)
    // {
    //     // Giả sử mục tiêu yêu cầu ít nhất X item loại này
    //     int requiredCount = 5; // Thay đổi dựa trên logic game của bạn
    //     return itemCounts[item] < requiredCount;
    // }
    //
    public void ItemMerged(string itemType)
    {
        if (itemCounts.ContainsKey(itemType))
            itemCounts[itemType] -= maxSlotPerTray;
    }
    
    public void AddItem(string itemType)
    {
        if (!itemCounts.TryAdd(itemType, 1))
        {
            itemCounts[itemType]++;
        }
    }

    public void SetMaxItemPerTray(int MaxCountPerTray)
    {
        maxSlotPerTray = MaxCountPerTray;
    }
    
    public void SetFindEmptySlotCallback(Func<int> callback)
    {
        this.GetEmptySlotCountCallback = callback;
    }

    public void SetCheckingItemOnQuestingCallback(Predicate<string> callback)
    {
        IsItemOnQuestingCallback = callback;
    }
}
