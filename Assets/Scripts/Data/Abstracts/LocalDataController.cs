using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LocalDataController<TDataModel> : DataController<TDataModel> where TDataModel : class, new()
{
    [SerializeField] protected string _filePath;

    protected string _fullPath;

    public string GetFullPath()
    {
        _fullPath = $"{Application.persistentDataPath}/{_filePath}.dat";

        return _fullPath;
    }

    public override Task LoadData()
    {
        var filePath = GetFullPath();

        TDataModel result = null;

        if (File.Exists(filePath))
        {
            try
            {
                var savedData = File.ReadAllText(filePath);
                result = JsonUtility.FromJson<TDataModel>(savedData);
                Debug.Log($"LoadData complete {filePath}\n{savedData}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Load data -- {filePath} -- is error: {ex.GetBaseException()}\n{ex.StackTrace}");
            }
        }

        if (result == null)
        {
            result = new TDataModel();
        }

        _data = result;

        return Task.CompletedTask;
    }


    public void SaveData()
    {
        var filePath = GetFullPath();

        try
        {
            var saveData = JsonUtility.ToJson(_data);
            File.WriteAllText(filePath, saveData);

            Debug.Log($"SaveData complete {filePath}\n{saveData}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save data -- {this.GetType()} -- is error: {ex.GetBaseException()}\n{ex.StackTrace}");
        }
    }

}
