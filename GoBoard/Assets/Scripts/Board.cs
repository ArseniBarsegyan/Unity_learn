using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private PlayerMover _player;
    [SerializeField] private GameObject goalPrefab;

    public static float spacing = 2f;
    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };

    public float drawGoalTime = 2f;
    public float drawGoalDelay = 2f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;

    public List<Node> AllNodes { get; private set; } = new List<Node>();
    public Node PlayerNode { get; private set; }
    public Node GoalNode { get; private set; }

    void Awake()
    {
        _player = FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();
        GoalNode = FindGoalNode();
    }

    public void GetNodeList()
    {
        var nList = GameObject.FindObjectsOfType<Node>();
        AllNodes = new List<Node>(nList);
    }

    public Node FindNodeAt(Vector3 position)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2(position.x, position.z));
        return AllNodes.Find(x => x.Coordinate == boardCoord);
    }

    public Node FindPlayerNode()
    {
        if (_player != null && !_player.isMoving)
        {
            return FindNodeAt(_player.transform.position);
        }

        return null;
    }

    private Node FindGoalNode()
    {
        return AllNodes.Find(x => x.isLevelGoal);
    }

    public void UpdatePlayerNode()
    {
        PlayerNode = FindPlayerNode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f,1f,1f,0.5f);
        if (PlayerNode != null)
        {
            Gizmos.DrawSphere(PlayerNode.transform.position, 0.2f);
        }
    }

    public void DrawGoal()
    {
        if (goalPrefab != null && GoalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, GoalNode.transform.position,
                Quaternion.identity);

            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType
                ));
        }
    }

    public void InitBoard()
    {
        if (PlayerNode != null)
        {
            PlayerNode.InitNode();
        }
    }
}
