using UnityEngine;
using UnityEngine.UI;

public class RotatingImageUI : MonoBehaviour
{
    [SerializeField] private Image rotateImage;
    [SerializeField] private float rotateZValue;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private float lerpingValue = 5;
    [SerializeField] private bool isEnable = false;

    public void SetRotateImg(Image image)
    {
        rotateImage = image;
    }

    public void SetEnable(bool isEnable)
    {
        this.isEnable = isEnable;
    }

    private void Update()
    {
        if (isEnable == false) return;
        if (rotateImage == null) return;

        rotateSpeed = AnimationManager.Instance.config.levelUIConfig.rotateSpeed;
        lerpingValue =  AnimationManager.Instance.config.levelUIConfig.lerpingValue;
        
        rotateZValue += Time.deltaTime * rotateSpeed;
        
        if (rotateZValue > 360)
            rotateZValue = 0;
        
        Quaternion nextQuater =
            Quaternion.Euler(
                rotateImage.transform.localRotation.x,
                rotateImage.transform.localRotation.y,
                rotateZValue);

        Quaternion lerping =
            Quaternion.Lerp(rotateImage.transform.localRotation
                , nextQuater
                , Time.deltaTime * lerpingValue);
        
        rotateImage.transform.localRotation = lerping;
    }
}