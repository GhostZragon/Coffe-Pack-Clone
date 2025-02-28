using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tray : MonoBehaviour
{
    [Header("Stand Point")] [SerializeField]
    private Transform[] points;

    public List<Item> items = new();
    [SerializeField] private Transform pointHolder;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private new Collider collider;
    [Header("Settings")] 
    [SerializeField] private int index;
    [SerializeField] private int maxItemCount;
    [Header("Gizmos")] 
    [SerializeField] private Vector3 size;
    [Header("Item settings")] 
    public int randomCount;
    public Transform Model;
    private const int OutsideSlotIndex = -1;
    private AlignSlotInTray alignSlotInTray;

    // [SerializeField] SerializableMotionSettings<Vector3, NoOptions> destroyMotionSettings;


    public int MaxCount
    {
        get => maxItemCount;
    }

    public int Index
    {
        get => index;
        set => index = value;
    }

    private void Awake()
    {
        alignSlotInTray = GetComponent<AlignSlotInTray>();
        collider = GetComponent<Collider>();
    }


    public void Add(Item item, bool isUsingAnimation = true)
    {
        if (items.Count == maxItemCount)
        {
            Debug.LogWarning("Already have full of item in tray, dont add more", gameObject);
            return;
        }

        if (!items.Contains(item))
        {
            items.Add(item);
        }

        // MoveAnimation(items.Count - 1, isUsingAnimation);
        SetStandPosition(isUsingAnimation);
    }


    public void Remove(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public bool CanAddMoreItem()
    {
        return items.Count < maxItemCount;
    }


    public HashSet<string> GetUniqueItemIDs()
    {
        HashSet<string> _tempUniqueIDs = new();
        foreach (var item in items)
        {
            _tempUniqueIDs.Add(item.itemID);
        }
        return _tempUniqueIDs;
    }

    public int GetCountOfItem(string itemID)
    {
        return items.Count(item => item.itemID == itemID);
    }

    public Item GetFirstOfItem(string itemID)
    {
        return items.FirstOrDefault(item => item.itemID == itemID);
    }

    private void SetStandPosition(bool isUsingAnimation)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Table.AddItemMoving();

            items[i].name = "Item_" + i;
            items[i].transform.parent = itemHolder;
            // items[i].transform.position = points[i].transform.position;
            items[i].transform.SetSiblingIndex(i);

            if (isUsingAnimation)
            {
                float delay = AnimationManager.Cur.config.itemcfg.itemTransferStartDelay + 0.1f * (i + 1);
                if (items[i].transform.position != points[i].transform.position)
                {
                    items[i].PlaySwapSound(delay);
                }
        
                Debug.Log($"Index: {i}",gameObject);
                LMotion.Create(items[i].transform.position, points[i].transform.position
                        , AnimationManager.Cur.config.itemcfg.itemTransferDuration)
                    .WithDelay(delay)
                    .WithEase(AnimationManager.Cur.config.itemcfg.itemTransferEase)
                    .WithOnComplete(Table.RemoveItemMoving)
                    .BindToPosition(items[i].transform);
                // AnimationManager.Instance.TransferItem(items[i].transform, points[i].position);
            }
            else
            {
                items[i].transform.position = points[i].transform.position;
                Table.RemoveItemMoving();
            }
        }
    }

    [Button]
    public void SetTrayToOriginalPosition()
    {
        if (CanBeDragged()) return;

        var stand = TrayManager.instance.GetStandPosition(index);
        transform.position = stand.position;
    }

    public void OnPickup()
    {
        collider.enabled = false;
    }

    public void OnRelease()
    {
        collider.enabled = true;
    }

    public void SetTrayToSlot()
    {
        if (index != OutsideSlotIndex)
        {
            index = OutsideSlotIndex;
            TrayManager.instance.Remove(this);
        }
    }

    public bool CanBeDragged()
    {
        return index == OutsideSlotIndex;
    }

    [Button]
    public void RequestItem()
    {
        if (ItemManager.Instance == null)
        {
            Debug.LogWarning("Item Manager is null", gameObject);
            return;
        }

        int count = ItemManager.Instance.GetRandomItemCount();
        for (int i = 0; i < count; i++)
        {
            var item = ItemManager.Instance.GetNewItem();
            if (item != null)
            {
                Add(item, false);
            }
        }
    }

    [Button]
    public void DestroyAnimation()
    {
        LMotion.Create(Model.localScale, Vector3.zero, AnimationManager.Cur.config.trayCfg.destroyTrayDuration)
            .WithEase(AnimationManager.Cur.config.trayCfg.destroyTrayEase)
            .WithOnComplete(() => { Destroy(gameObject); })
            .BindToLocalScale(Model);
    }

    public bool IsFullOfItem(out string itemID)
    {
        if (items.Count == 0)
        {
            itemID = string.Empty;
            return false;
        }

        var localItemID = items[0].itemID;
        itemID = localItemID;
        return items.All(item => item.itemID == localItemID) && items.Count == maxItemCount;
    }

    public void SetMaxCount(int maxCountPerTray)
    {
        maxItemCount = maxCountPerTray;
        alignSlotInTray.Alin(maxCountPerTray);
    }
}