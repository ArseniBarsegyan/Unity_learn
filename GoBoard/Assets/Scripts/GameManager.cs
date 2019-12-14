using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Board board;
    private PlayerManager player;

    public bool HasLevelStarted { get; }
    public bool IsGamePlaying { get; }
    public bool IsGameOver { get; }
    public bool HasLevelFinished { get; }

    public float Delay = 1f;

    void Awake()
    {
        board = FindObjectOfType<Board>().GetComponent<Board>();
        player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (player != null && board != null)
        {
            StartCoroutine(RunGameLoop());
        }
        else
        {
            Debug.LogWarning("Error: no player or board found!");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine(StartLevelRoutine());
        yield return StartCoroutine(PlayLevelRoutine());
        yield return StartCoroutine(EndLevelRoutine());
    }

    IEnumerator StartLevelRoutine()
    {
        yield return null;
    }

    IEnumerator PlayLevelRoutine()
    {
        yield return null;
    }

    IEnumerator EndLevelRoutine()
    {
        yield return null;
    }
}
