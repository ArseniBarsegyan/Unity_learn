using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }

    private List<IGameManager> _startSequence;

    void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        _startSequence = new List<IGameManager> {Player, Inventory};

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        NetworkService network = new NetworkService();

        foreach (var manager in _startSequence)
        {
            manager.Startup(network);
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
