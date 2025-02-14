using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class CollectorUI : MonoBehaviour
{
    [SerializeField] private Transform ImageTransform;
    [SerializeField] private Image fillAmountImg;
    [SerializeField] private Image shadowFillAmountImg;

    private void Awake()
    {
        fillAmountImg.fillAmount = 0;
    }

    public Vector3 GetIconPosition()
    {
        return ImageTransform.TransformPoint(ImageTransform.position);
    }

    public void FillAmount(float value)
    {
        LMotion.Create(fillAmountImg.fillAmount, value, 0.15f)
            .WithDelay(0.1f)
            .BindToFillAmount(fillAmountImg);
    }
}
