using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    private NetworkService _networkService;

    public ManagerStatus Status { get; private set; }
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }

    public void Startup(NetworkService networkService)
    {
        Debug.Log("Mission manager starting...");
        _networkService = networkService;
        CurrentLevel = 0;
        MaxLevel = 1;
        Status = ManagerStatus.Started;
    }

    public void GoToNext()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
            var sceneName = "Level" + CurrentLevel;
            Debug.Log("Loading " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Last level");
        }
    }

    public void ReachObjective()
    {
        Messenger.Broadcast(GameEvent.LevelCompleted);
    }
}
