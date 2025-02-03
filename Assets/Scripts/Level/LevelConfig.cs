using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "cfg_level ",menuName = "SO/Level Config")]
public class LevelConfig : ScriptableObject
{
    public TextAsset LevelCSV;
    public PuzzleQuestData PuzzleQuestData;
}