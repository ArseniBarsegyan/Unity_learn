using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Vector2 m_coordinate;
    public Vector2 Coordinate => Utility.Vector2Round(m_coordinate);

    public List<Node> NeighborNodes { get; private set; } = new List<Node>();

    Board m_board;

    private bool m_isInitialized;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>();
        m_coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    [SerializeField] private GameObject geometry;
    public float scaleTime = 0.3f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public bool autoRun;

    public float delay = 1f;

    void Start()
    {
        if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;
        }
        if (autoRun)
        {
            InitNode();
        }
        if (m_board != null)
        {
            NeighborNodes = FindNeighbors(m_board.AllNodes);
        }
    }

    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
                ));
        }
    }

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();

        foreach(var dir in Board.directions)
        {
            var foundNeighbor = nodes.Find(x => x.Coordinate == Coordinate + dir);

            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }
        return nList;
    }

    public void InitNode()
    {
        if (!m_isInitialized)
        {
            ShowGeometry();
            InitNeighbors();
            m_isInitialized = true;
        }
    }

    private void InitNeighbors()
    {
        StartCoroutine(InitNeighborRoutine());
    }

    private IEnumerator InitNeighborRoutine()
    {
        yield return new WaitForSeconds(delay);

        foreach(var n in NeighborNodes)
        {
            n.InitNode();
        }
    }
}
