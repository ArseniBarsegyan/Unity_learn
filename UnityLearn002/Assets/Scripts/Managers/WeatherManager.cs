using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;
    public ManagerStatus Status { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        _network = service;
        StartCoroutine(_network.GetWeather(OnDataLoaded));
        Status = ManagerStatus.Started;
    }

    public void OnDataLoaded(string data)
    {
        Debug.Log(data);
        Status = ManagerStatus.Started;
    }
}
