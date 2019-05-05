using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImagesManager))]
public class Managers : MonoBehaviour
{
    private List<IGameManager> _startSequence;
    public static WeatherManager Weather { get; private set; }
    public static ImagesManager Images { get; private set; }

    void Awake()
    {
        Weather = GetComponent<WeatherManager>();
        Images = GetComponent<ImagesManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Weather);
        _startSequence.Add(Images);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        var networkService = new NetworkService();
        foreach (var gameManager in _startSequence)
        {
            gameManager.Startup(networkService);
        }
        yield return null;
        var numModules = _startSequence.Count;
        var numReady = 0;
        while (numReady < numModules)
        {
            var lastReady = numReady;
            numReady = 0;
            foreach (var manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
            }
            yield return null;
        }
        Debug.Log("All managers started up");
    }
}
