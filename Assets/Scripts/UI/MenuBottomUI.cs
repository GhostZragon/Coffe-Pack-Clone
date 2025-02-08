using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MenuBottomUI : MonoBehaviour
{
    [SerializeField] private Texture[] sprites;
    [SerializeField] private Color[] colors;
    [SerializeField] private RawImage scrollerImage;
    [SerializeField] private RawImage colorImage;
    [SerializeField] private Color color;
    private int index = 0;
    
    [Button]
    private void Test()
    {
        if (index >= sprites.Length)
            index = 0;
        scrollerImage.texture = sprites[index++];
        
        colorImage.color = colors[index];
    }
}