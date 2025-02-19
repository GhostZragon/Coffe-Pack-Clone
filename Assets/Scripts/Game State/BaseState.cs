using System;
using UnityEngine;
[Serializable]
public abstract class BaseState
{
    private readonly GameStateManager stateManager = GameStateManager.Instance;
    
    public virtual void PrepareState()
    {
        CatchRef();
        Register();
        
    }

    public virtual void DestroyState()
    {
        UnRegister();
    }

    protected virtual void Register()
    {
        
    }

    protected virtual void UnRegister()
    {
        
    }

    protected virtual void CatchRef()
    {
        
    }
    
    
    protected void ChangeState(BaseState newState)
    {
        stateManager.ChangeState(newState);
    }
}