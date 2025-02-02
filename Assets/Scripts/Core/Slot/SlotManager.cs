using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance;
    [SerializeField] private SlotBase normalSlot;
    [SerializeField] private SlotBase blockingSlot;

    private void Awake()
    {
        Instance = this;
    }


    public SlotBase GetSlot(SlotType slotType)
    {
        var prefab = GetSlotPrefab(slotType);

        if (prefab == null) return null;

        return Instantiate(prefab, transform);
    }

    private SlotBase GetSlotPrefab(SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Normal:
                return normalSlot;
            case SlotType.Blocking:
                return blockingSlot;
        }

        return null;
    }
}