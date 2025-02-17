using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseView
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private bool isTestingChangeScreen = false;

    private void Update()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }
}
