using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateWithSubStates : BaseState
{
    protected ISubState currentSubState;
    protected readonly Dictionary<Type, ISubState> subStates = new();

    protected void RegisterSubState<T>(T state) where T : ISubState
    {
        Type stateType = typeof(T);

        subStates[stateType] = state;
        Debug.Log($" Registered sub-state: {stateType.Name} ");
    }

    protected void ChangeSubState<T>() where T : ISubState
    {
        if (currentSubState != null)
        {
            currentSubState.Exit();
        }

        currentSubState = subStates[typeof(T)];
        currentSubState.Enter();
        LogCurrentState();
    }
    protected void LogCurrentState()
    {
        string currentStateName = currentSubState?.GetType().Name ?? "No active sub-state";
        Debug.Log($"[{currentSubState}] Current sub-state: {currentStateName}");
    }
}