using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ServerDataController<TDataModel> : DataController<TDataModel> where TDataModel : class, new()
{
    protected const string BASE_URL = "https://example.com";
    protected const int MAX_RETRY_COUNT = 3;

    [SerializeField] protected string _path;

    protected string _fullPath;

    public string GetFullPath()
    {
        if (string.IsNullOrEmpty(_fullPath))
        {
            _fullPath = $"{BASE_URL}{_path}";
        }

        return _fullPath;
    }
    protected abstract UnityWebRequest GetRequest();

    public override async Task LoadData()
    {
        try
        {
            bool isSuccess = false;
            string lastError = null;
            string txtResult = null;
            for (int i = 0; i < MAX_RETRY_COUNT; i++)
            {
                UnityWebRequest webRequest = GetRequest();
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }


                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        lastError = webRequest.error;
                        isSuccess = false;
                        Debug.LogError($"Loading data error: {lastError}");
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        lastError = webRequest.error;
                        isSuccess = false;
                        Debug.LogError($"HTTP error while loading data: {lastError}");
                        break;
                    case UnityWebRequest.Result.Success:
                        isSuccess = true;
                        txtResult = webRequest.downloadHandler.text;
                        Debug.Log($"Data received: {txtResult}");
                        break;
                }


                if (isSuccess)
                {
                    break;
                }
            }

            if (isSuccess)
            {
                TDataModel result = JsonConvert.DeserializeObject<TDataModel>(txtResult);
                _data = result;
            }
            else
            {
                //TODO: notify the client to handle UI warning process
                Debug.LogError($"Loading data is out of retry: {lastError}");
                _data = new TDataModel();
            }
        }
        catch (Exception ex)
        {
            //TODO: notify the client to handle UI warning process
            Debug.LogError($"Loading data is error with unknown reason: {ex.GetBaseException()}\n{ex.StackTrace}");
            _data = new TDataModel();
        }
    }
}
