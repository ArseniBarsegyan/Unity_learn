using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private InventoryPopup popup;

    void Awake()
    {
        Messenger.AddListener(GameEvent.HealthUpdated, OnHealthUpdated);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HealthUpdated, OnHealthUpdated);
    }

    void Start()
    {
        OnHealthUpdated();
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
}
