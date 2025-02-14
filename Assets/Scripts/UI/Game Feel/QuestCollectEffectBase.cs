using Sirenix.OdinInspector;
using UnityEngine;

public abstract class QuestCollectEffectBase : MonoBehaviour
{
    [SerializeField] protected GameObject trailPrefab;
    [Button]
    public abstract void CreateTrail(Vector3 spawnPos);
}