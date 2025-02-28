using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-99)]
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private List<Item> availableItems = new();
    private Dictionary<string, float> itemSpawnWeights = new Dictionary<string, float>(); // Trọng số random
    private Dictionary<string, int> itemSpawnCounts = new Dictionary<string, int>(); // Số lượng từng item trên bàn

    private Dictionary<string, Item> itemStorage = new();
  
    private void Awake()
    {
        Instance = this;
        

        foreach (var item in Resources.LoadAll<Item>("Prefabs/Items"))
        {
            itemStorage.Add(item.itemID,item);
        }
    }


    private int totalSlots;
    private int maxSlotPerTray = 4;
  
    private Func<int> GetEmptySlotCountCallback;
    private Predicate<string> IsItemOnQuestingCallback;

  

    public Item GetNewItem()
    {
        var itemPrefabID = GetRandomItemID();
        var itemPrefab = GetItemPrefab(itemPrefabID);
        Debug.Log("Add item id " +itemPrefabID);
        if (itemPrefab == null) return null;
        
        var item = Instantiate(itemPrefab);
        IncrementItemCount(item.itemID);
        return item;
    }

    private Item GetItemPrefab(string itemID)
    {
        itemStorage.TryGetValue(itemID, out var item);
        return item;
    }

    public string GetRandomItemID()
    {
        UpdateSpawnWeights();

        // Chọn item dựa trên trọng số
        float totalWeight = itemSpawnWeights.Values.Sum();
        float randValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0;

        foreach (var item in itemSpawnWeights)
        {
            cumulative += item.Value;
            if (randValue <= cumulative)
                return item.Key;
        }

        return availableItems[0].itemID; // Dự phòng (không bao giờ xảy ra)
    }
    [Header("Difficult Settings")] 
    [SerializeField] private float existingItemWeightMultiplier = 1.5f; // Trọng số khi item đã tồn tại trên bàn
    [SerializeField] private float newItemWeightReductionWhenFull = 0.5f; // Giảm trọng số item mới khi bàn đầy
    [SerializeField] private float questItemWeightMultiplier = 1.5f; // Trọng số khi item cần để hoàn thành nhiệm vụ

    private void UpdateSpawnWeights()
    {
        totalSlots = GetEmptySlotCountCallback.Invoke();
        itemSpawnWeights.Clear();
    
        int totalPlacedItems = itemSpawnCounts.Values.Sum();

        foreach (var item in availableItems)
        {
            float weight = 1.0f;

            // Tăng trọng số nếu item đã có trên bàn nhưng chưa đạt giới hạn merge
            if (itemSpawnCounts[item.itemID] > 0 && itemSpawnCounts[item.itemID] < maxSlotPerTray)
                weight *= existingItemWeightMultiplier;

            // Giảm trọng số spawn item mới nếu bàn gần đầy
            if (totalPlacedItems >= totalSlots * 0.8f && itemSpawnCounts[item.itemID] == 0)
                weight *= newItemWeightReductionWhenFull;

            // Tăng trọng số nếu item cần thiết để hoàn thành nhiệm vụ
            if (IsItemOnQuestingCallback(item.itemID))
                weight *= questItemWeightMultiplier;

            itemSpawnWeights[item.itemID] = weight;
        }
    }

    public void HandleItemMerged(string itemType)
    {
        if (itemSpawnCounts.ContainsKey(itemType))
            itemSpawnCounts[itemType] -= maxSlotPerTray;
    }

    private void IncrementItemCount(string itemType)
    {
        if (!itemSpawnCounts.TryAdd(itemType, 1))
        {
            itemSpawnCounts[itemType]++;
        }
    }

    public void SetMaxItemsPerSlot(int MaxCountPerTray)
    {
        maxSlotPerTray = MaxCountPerTray;
    }
    
    public void SetEmptySlotCountCallback(Func<int> callback)
    {
        this.GetEmptySlotCountCallback = callback;
    }

    public void SetQuestItemCheckCallback(Predicate<string> callback)
    {
        IsItemOnQuestingCallback = callback;
    }
    
    [SerializeField] private float exponentValue = 0.7f;
    public int GetRandomItemCount()
    {
        int count = GenerateExponentialBiasRandom(1,maxSlotPerTray/2, exponentValue);
        return count;
    }

    [Button]
    private void TestRandomFucn(int min,int max,float exponent)
    {
        Debug.Log("ExponentialBiasRandom: "+GenerateExponentialBiasRandom(min,max,exponent));
    }    
    
    int GenerateExponentialBiasRandom(int min, int max, float exponent = 2.0f)
    {
        float rand = Mathf.Pow(UnityEngine.Random.value, exponent);
        return Mathf.RoundToInt(Mathf.Lerp(min, max, rand));
    }

    public void InitializeAvailableItems(List<string> uniqueItemID)
    {
        availableItems.Clear();
        itemSpawnCounts.Clear();
        if(uniqueItemID.Count == 0)
            Debug.LogWarning("This is error ");
        foreach (var item in uniqueItemID)
        {
            if (itemStorage.TryGetValue(item, out var prefab))
            {
                Debug.Log($"Adding: {item}",gameObject);
                availableItems.Add(prefab);
                itemSpawnCounts[prefab.itemID] = 0;
            }
            else
            {
                Debug.Log($"Dont have: {item}",gameObject);
            }
        }
    }
}
