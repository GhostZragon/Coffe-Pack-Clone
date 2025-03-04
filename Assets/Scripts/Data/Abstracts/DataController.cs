using System.Threading.Tasks;
using UnityEngine;

public abstract class DataController : ScriptableObject
{
    public abstract Task LoadData();
    public abstract void InitData();
}
public abstract class DataController<TDataModel> : DataController where TDataModel : class, new()
{
    public delegate void DataChangedHandler(TDataModel data);
    public delegate void SpecificValueChangedHandler(int newValue);

    protected TDataModel _data;

    public override void InitData()
    {
        _data = new TDataModel();
    }
}
