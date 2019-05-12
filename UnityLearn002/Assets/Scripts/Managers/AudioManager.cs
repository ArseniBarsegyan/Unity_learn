using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;
    public ManagerStatus Status { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager starting...");
        _network = service;

        Status = ManagerStatus.Started;
    }
}
