using Sirenix.OdinInspector;
using UnityEngine;

public class Client : MonoBehaviour
{
    public GameData gameData;
    private void Start()
    {
        DataManager.Instance.LoadData();
    }

    [Button]
    private void TryGetData()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        GameDataController exampleDataController = DataManager.Instance.GetDataController<GameDataController>();
        Debug.Log(exampleDataController.GetDisplayName());
    }
    [Button]
    private void SetData()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        var exampleDataController = DataManager.Instance.GetDataController<GameDataController>();
        exampleDataController.SetLives(Random.Range(0, 100));
        Debug.Log(exampleDataController.GetCurrency());
        exampleDataController.SaveData();
    }


}
