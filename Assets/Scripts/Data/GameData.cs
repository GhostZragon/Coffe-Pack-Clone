using UnityEngine;
public class GameData
{
    [JsonProperty][SerializeField] private string displayName;
    [JsonProperty][SerializeField] private int currency;
    [JsonProperty][SerializeField] private int lives;

    public string DisplayName { get => displayName; }
    public int Currency { get => currency; }
    public int Lives { get => lives; }

}
