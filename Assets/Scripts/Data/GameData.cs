using System;
using UnityEngine;
[Serializable]
public class GameData
{
    [SerializeField] private string displayName;
    [SerializeField] private int currency;
    [SerializeField] private int lives;

    public string DisplayName { get => displayName; set => displayName = value; }
    public int Currency { get => currency; set => currency = value; }
    public int Lives { get => lives; set => lives = value; }
}
