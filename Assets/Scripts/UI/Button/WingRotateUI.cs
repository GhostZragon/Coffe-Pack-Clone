using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WingRotateUI : MonoBehaviour
{
    [SerializeField] private Image leftWing;
    [SerializeField] private Image rightWing;
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private float flapTime = 0.5f;
    [SerializeField] private int flapCount = 5;
    private CompositeMotionHandle handle = new();
    private void Awake()
    {
    }
    [Button]    
    private void TweenWing()
    {
        handle.Cancel();
        
        var tween1 = LMotion.Create(Quaternion.Euler(0, 0, min), Quaternion.Euler(0, 0, max), flapTime)
            .WithEase(Ease.InOutSine)
            .WithLoops(-1, LoopType.Yoyo).BindToLocalRotation(leftWing.transform);
        var tween2 = LMotion.Create(Quaternion.Euler(0, 0, max), Quaternion.Euler(0, 0, min), flapTime)
            .WithEase(Ease.InOutSine)
            .WithLoops(-1, LoopType.Yoyo).BindToLocalRotation(rightWing.transform);
        // handle.Add(tween1);
        // handle.Add(tween2);
        
    }
}