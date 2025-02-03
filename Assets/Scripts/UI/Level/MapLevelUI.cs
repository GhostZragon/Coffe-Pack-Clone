using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelUI : MonoBehaviour
{
    [SerializeField] private Image[] MapImages;

    [SerializeField] private int mapIndex;
    
    
    
    [Button]
    private void ActiveMap(int startLevel)
    {
        for (int i = 0; i < MapImages.Length; i++)
        {
            MapImages[i].gameObject.SetActive(i == mapIndex);

            if (i == mapIndex)
            {
                // load all button

                var buttons = MapImages[i].transform.GetComponentsInChildren<LevelUI>();

                foreach (var button in buttons)
                {
                    button.Setup(startLevel++,false);
                }
            }
        }
    }
}
