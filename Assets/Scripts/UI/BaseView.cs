using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] protected GameObject view;

    protected virtual void Awake()
    {
        if (view == null)
            view = gameObject;
        Register();
    }

    protected void OnDestroy()
    {
        UnRegister();
    }

    protected virtual void Register()
    {
        
    }

    protected virtual void UnRegister()
    {
        
    }
    public virtual void Show()
    {
        view.SetActive(true);
    }

    public virtual void Hide()
    {
        view.SetActive(false);
    }

 
}