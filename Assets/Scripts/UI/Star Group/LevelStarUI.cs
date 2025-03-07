using LitMotion;
using LitMotion.Extensions;
using System;
using UnityEngine;
public class LevelStarUI : LevelStarUIBase
{
    [SerializeField] private bool canAnimated = false;
    [SerializeField] private float timer = 0;
    [SerializeField] private float animatedTimer = 2f;
    public void SetAnimated(bool canAnimated)
    {
        this.canAnimated = canAnimated;
        animatedTimer = 0;
    }
}