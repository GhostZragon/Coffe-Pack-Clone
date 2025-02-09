using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class StateUIManager : MonoBehaviour
{
    private IEnter[] enters;
    private IExit[] exits;

    private void Awake()
    {
        enters = GetComponentsInChildren<IEnter>();
        exits = GetComponentsInChildren<IExit>();
    }

    private void Start()
    {
        EnterAll();
    }

    [Button]
    private void EnterAll()
    {
        enters.ForEach(item => item.Enter());
    }
    
    [Button]
    private void ExitAll()
    {
        exits.ForEach(item => item.Exit());
    }
}