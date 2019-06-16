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
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HealthUpdated, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LevelCompleted, OnLevelComplete);
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

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";
        yield return new WaitForSeconds(2);
        Managers.Mission.GoToNext();
    }
}
