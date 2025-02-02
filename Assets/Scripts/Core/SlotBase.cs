
using UnityEngine;

public abstract class SlotBase : MonoBehaviour
{
    
    public abstract bool CanPlacedTray();
    public abstract void ActiveSpecialEffect();

    public virtual void PlayClearAnimation()
    {
        Destroy(gameObject);
    }
    
}