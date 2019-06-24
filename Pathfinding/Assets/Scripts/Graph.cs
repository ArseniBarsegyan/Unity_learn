using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    public static readonly Vector2[] AllDirections =
    {
        new Vector2(0f,1f), 
        new Vector2(1f,1f), 
        new Vector2(1f,0f), 
        new Vector2(1f,-1f), 
        new Vector2(0f,-1f), 
        new Vector2(-1f,-1f), 
        new Vector2(-1f,0f), 
        new Vector2(-1f,1f), 
    };

    private int[,] _mapData;
    private int _width;
    private int _height;

    public void Init(int[,] mapData)
    {
        _mapData = mapData;
        _width = mapData.GetLength(0);
        _height = mapData.GetLength(1);

        nodes = new Node[_width, _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                NodeType nodeType = (NodeType) _mapData[x, y];
                Node newNode = new Node(x, y, nodeType);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x,0,y);

                if (nodeType == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (nodes[x, y].NodeType != NodeType.Blocked)
                {
                    nodes[x, y].neighbors = GetNeighbors(x, y);
                }
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        foreach (var dir in directions)
        {
            int newX = x + (int) dir.x;
            int newY = y + (int) dir.y;

            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null
                                           && nodeArray[newX, newY].NodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }

        return neighborNodes;
    }

    List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, AllDirections);
    }
}
