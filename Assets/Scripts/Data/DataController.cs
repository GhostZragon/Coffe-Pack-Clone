using System.Threading.Tasks;
using UnityEngine;

public abstract class DataController : ScriptableObject
{
    public abstract Task LoadData();
    public abstract void InitData();
}
public abstract class DataController<TDataModel> : DataController where TDataModel : class, new()
{
    protected TDataModel _data;

    public override void InitData()
    {
        _data = new TDataModel();
    }
}
