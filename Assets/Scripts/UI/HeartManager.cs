using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private int maxCount;
    [SerializeField] private HeartUI HeartPrefab;
    [SerializeField] private GameObject container;

    private List<HeartUI> hearts = new();

    private void Awake()
    {
        CreateHeart();
    }

    private void CreateHeart()
    {
        for (int i = 0; i < maxCount; i++)
        {
            var heart = Instantiate(HeartPrefab, container.transform);
            hearts.Add(heart);
        }
    }
}
