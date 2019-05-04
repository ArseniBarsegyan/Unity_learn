using System.Collections.Generic;
using Assets.Scripts.Broadcast;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;
    public ManagerStatus Status { get; private set; }
    public float CloudValue { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        _network = service;
        StartCoroutine(_network.GetWeather(OnDataLoaded));
        Status = ManagerStatus.Started;
    }

    public void OnDataLoaded(string data)
    {
        if (Json.Deserialize(data) is Dictionary<string, object> dictionary)
        {
            if (dictionary["clouds"] is Dictionary<string, object> clouds)
            {
                CloudValue = (long) clouds["all"] / 100f;
            }
        }
        Debug.Log("Value: " + CloudValue);
        Messenger.Broadcast(GameEvent.WeatherUpdated);
        Status = ManagerStatus.Started;
    }
}
