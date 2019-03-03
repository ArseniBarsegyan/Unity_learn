using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private int _score;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private SettingsPopup settingsPopup;

    void Awake()
    {
        Messenger.AddListener(GameEvent.EnemyHit, OnEnemyHit);
    }

    void Destroy()
    {
        Messenger.RemoveListener(GameEvent.EnemyHit, OnEnemyHit);
    }
    private void OnEnemyHit()
    {
        _score++;
        scoreLabel.text = _score.ToString();
    }

    void Start()
    {
        _score = 0;
        scoreLabel.text = _score.ToString();
        settingsPopup.Close();
    }

    public void OnOpenSettings()
    {
        settingsPopup.Open();
    }

    public void OnPointerDown()
    {
        Debug.Log("pointer down");
    }
}
