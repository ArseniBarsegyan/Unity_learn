using System.Collections;
using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private InventoryPopup popup;
    [SerializeField] private Text levelEnding;

    void Awake()
    {
        Messenger.AddListener(GameEvent.HealthUpdated, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LevelCompleted, OnLevelComplete);
        Messenger.AddListener(GameEvent.LevelFailed, OnLevelFailed);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HealthUpdated, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LevelCompleted, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LevelFailed, OnLevelFailed);
    }

    void Start()
    {
        OnHealthUpdated();
        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
    }

    public void SaveGame()
    {
        Managers.Data.SaveGameState();
    }

    public void LoadGame()
    {
        Managers.Data.LoadGameState();
    }

    private void OnHealthUpdated()
    {
        var message = "Health: " + Managers.Player.Health + "/" +
                         Managers.Player.MaxHealth;
        healthLabel.text = message;
    }

    private void OnLevelComplete()
    {
        StartCoroutine(CompleteLevel());
    }

    private void OnLevelFailed()
    {
        StartCoroutine(FailLevel());
    }

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";
        yield return new WaitForSeconds(2);
        Managers.Mission.GoToNext();
    }

    private IEnumerator FailLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed";
        yield return new WaitForSeconds(2);
        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent();
    }
}
