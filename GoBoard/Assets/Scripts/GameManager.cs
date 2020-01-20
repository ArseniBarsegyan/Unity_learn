using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Board board;
    private PlayerManager player;

    public bool HasLevelStarted { get; private set; }
    public bool IsGamePlaying { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool HasLevelFinished { get; set; }

    public float Delay = 1f;

    public UnityEvent StartLevelEvent;
    public UnityEvent PlayLevelEvent;
    public UnityEvent EndLevelEvent;

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
        Debug.Log("Start level");
        player.playerInput.InputEnabled = false;
        while (!HasLevelStarted)
        {
            // show start screen
            yield return null;
        }

        StartLevelEvent?.Invoke();
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("Play level");
        IsGamePlaying = true;
        yield return new WaitForSeconds(Delay);
        player.playerInput.InputEnabled = true;

        PlayLevelEvent?.Invoke();
        while (!IsGameOver)
        {
            yield return null;
            IsGameOver = IsWinner();
        }
        Debug.Log("WIN");
    }

    private bool IsWinner()
    {
        if (board.PlayerNode != null)
        {
            return board.PlayerNode == board.GoalNode;
        }

        return false;
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("End level");

        player.playerInput.InputEnabled = false;
        EndLevelEvent?.Invoke();
        // show end screen
        while (!HasLevelFinished)
        {
            yield return null;
        }

        RestartLevel();
    }

    private void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        HasLevelStarted = true;
    }
}
