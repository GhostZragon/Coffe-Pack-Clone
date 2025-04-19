using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[DefaultExecutionOrder(-99)]
public class DataManager : MonoBehaviour
{
    [SerializeField] private DataController[] _controllers;
    private Dictionary<Type, DataController> _typeToControllerMap;
    public static DataManager Instance;
    private void Awake()
    {
        Instance = this;

        _typeToControllerMap = new();

        foreach(var controller in _controllers)
        {
            _typeToControllerMap.Add(controller.GetType(), controller);
        }

    }

    public Task LoadData()
    {
        List<Task> loadingTasks = new List<Task>(_controllers.Length);

        foreach (var controller in _controllers)
        {
            Task loadingTask = controller.LoadData();
            loadingTasks.Add(loadingTask);
        }

        return Task.WhenAll(loadingTasks);

    }


    public TDataController GetDataController<TDataController>() where TDataController : DataController
    {
        try
        {
            return _typeToControllerMap[typeof(TDataController)] as TDataController;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Can't find data controller of type {typeof(TDataController)}: {ex.GetBaseException()}\n{ex.StackTrace}");
            return null;
        }
    }

}
