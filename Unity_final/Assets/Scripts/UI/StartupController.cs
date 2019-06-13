using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    void Awake()
    {
        Messenger<int, int>.AddListener(StartupEvent.ManagersProgress,
            OnManagersProgress);
        Messenger.AddListener(StartupEvent.ManagersStarted,
            OnManagersStarted);
    }
    void OnDestroy()
    {
        Messenger<int, int>.RemoveListener(StartupEvent.ManagersProgress,
            OnManagersProgress);
        Messenger.RemoveListener(StartupEvent.ManagersStarted,
            OnManagersStarted);
    }

    private void OnManagersProgress(int numReady, int numModules)
    {
        float progress = (float)numReady / numModules;
        progressBar.value = progress;
    }

    private void OnManagersStarted()
    {
        Managers.Mission.GoToNext();
    }
}
