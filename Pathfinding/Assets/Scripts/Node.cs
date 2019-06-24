using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node
{
    public NodeType NodeType = NodeType.Open;

    public int XIndex = -1;
    public int YIndex = -1;

    public Vector3 position;

    public List<Node> Neighbors = new List<Node>();
    public Node Previous = null;

    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        XIndex = xIndex;
        YIndex = yIndex;
        NodeType = nodeType;
    }

    public void Reset()
    {
        Previous = null;
    }
}
