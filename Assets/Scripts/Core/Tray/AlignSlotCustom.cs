using UnityEngine;

public class AlignSlotCustom : AlignSlotInTray
{
    [SerializeField] private Vector3 planeSize;
    protected override Vector3 GetPlaneSize()
    {
        return planeSize;
    }
}