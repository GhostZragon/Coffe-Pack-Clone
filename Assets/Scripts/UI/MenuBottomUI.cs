using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MenuBottomUI : MonoBehaviour
{
    [SerializeField] private Texture[] sprites;
    [SerializeField] private RawImage rawImage;
    private int index = 0;
    
    [Button]
    private void Test()
    {
        if (index >= sprites.Length)
            index = 0;
        rawImage.texture = sprites[index++];
    }
}