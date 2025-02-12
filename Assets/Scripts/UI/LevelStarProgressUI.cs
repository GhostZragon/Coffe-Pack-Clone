using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class LevelStarProgressUI : LevelStarUI
{
    [SerializeField] private int Frequency = 2;
    [SerializeField] private int Damping_Ratio  = 10;
    public void CollectPointEffect(int currentStage)
    {
        var starTransform = images[currentStage].transform;

        LMotion.Punch.Create(Vector3.one,Vector3.one * 1.05f, 0.2f)
            .WithFrequency(Frequency)
            .WithDampingRatio(Damping_Ratio)
            .BindToLocalScale(starTransform);
    }
}