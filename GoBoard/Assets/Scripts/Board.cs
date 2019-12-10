using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static float spacing = 2f;
    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };

    public List<Node> AllNodes { get; private set; } = new List<Node>();

    void Awake()
    {
        GetNodeList();
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
}
