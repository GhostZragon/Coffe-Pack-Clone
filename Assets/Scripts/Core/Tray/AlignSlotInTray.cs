using UnityEngine;

public class AlignSlotInTray : AlignItemBase
{
    protected override Vector3 GetPlaneSize()
    {
        return transform.localScale;
    }
}