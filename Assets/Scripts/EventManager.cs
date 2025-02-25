using System;
using UnityEngine;

public class EventManager
{
}

public struct ItemInfo
{
    public ItemInfo(string _itemId, Vector3 worldPositionPos)
    {
        ItemId = _itemId;
        WorldPosition = worldPositionPos;
    }
    public string ItemId;
    public Vector3 WorldPosition;
}