using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelUI : MonoBehaviour
{
    [SerializeField] private Image[] MapImages;

    [SerializeField] private int mapIndex;
    
    [Button]
    private void ActiveMap()
    {
        for (int i = 0; i < MapImages.Length; i++)
        {
            MapImages[i].gameObject.SetActive(i == mapIndex);
        }
    }
}
