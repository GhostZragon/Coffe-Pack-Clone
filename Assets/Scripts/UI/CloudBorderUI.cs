using UnityEngine;
using UnityEngine.UI;

public class CloudBorderUI : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    private void Awake()
    {
        horizontalLayoutGroup.enabled = true;
    }

    private void Start()
    {
        horizontalLayoutGroup.enabled = false;
    }
}
