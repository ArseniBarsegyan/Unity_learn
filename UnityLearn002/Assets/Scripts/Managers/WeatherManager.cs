using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;
    public ManagerStatus Status { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        _network = service;
        Status = ManagerStatus.Started;
    }
}
