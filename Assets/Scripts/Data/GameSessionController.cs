using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSessionController", menuName = "Data/Game Session Controller")]
public class GameSessionController: RuntimeDataController<GameSessionData>
{
    public override Task LoadData()
    {
        InitData();
        return Task.CompletedTask;
    }

    public void ResetSession()
    {
        InitData();
    }

    public GameSessionData Data => _data;
}