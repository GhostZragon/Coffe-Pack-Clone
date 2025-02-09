using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class AvatarUI : MonoBehaviour, IEnter
{
    private Vector3 defautlScale;

    private void Awake()
    {
        defautlScale = transform.localScale;
    }

    public void Enter()
    {
        LMotion.Create(Vector3.zero, defautlScale * 1.2f, AnimationManager.Cur.config.topUIConfig.avatarScaleTime)
            .WithEase(AnimationManager.Cur.config.topUIConfig.scaleUpEase).WithOnComplete(() =>
            {
                LMotion.Create(transform.localScale, defautlScale, AnimationManager.Cur.config.topUIConfig.avatarScaleTime)
                    .WithEase(AnimationManager.Cur.config.topUIConfig.scaleDownEase)
                    .BindToLocalScale(transform);
            })
            .BindToLocalScale(transform);
    }
}
