using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Node _startNode;
    private Node _goalNode;
    private Graph _graph;
    private GraphView _graphView;

    private Queue<Node> _frontierNodes;
    private List<Node> _exploredNodes;
    private List<Node> _pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    private int _iterations = 0;
    public bool IsComplete = false;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER INIT ERROR: missing components");
            return;
        }

        if (start.NodeType == NodeType.Blocked || goal.NodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER INIT ERROR: start and goal nodes must be unblocked");
            return;
        }

        _graphView = graphView;
        _graph = graph;
        _startNode = start;
        _goalNode = goal;

        ShowColors(graphView, start, goal);

        _frontierNodes = new Queue<Node>();
        _frontierNodes.Enqueue(start);
        _exploredNodes = new List<Node>();
        _pathNodes = new List<Node>();

        for (int x = 0; x < _graph.Width; x++)
        {
            for (int y = 0; y < _graph.Height; y++)
            {
                _graph.nodes[x,y].Reset();
            }
        }

        IsComplete = false;
        _iterations = 0;
    }

    private void ShowColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        if (_frontierNodes != null)
        {
            graphView.ColorNodes(_frontierNodes.ToList(), frontierColor);
        }

        if (_exploredNodes != null)
        {
            graphView.ColorNodes(_exploredNodes, exploredColor);
        }

        NodeView startNodeView = graphView.NodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.NodeViews[goal.xIndex, goal.yIndex];
        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    private void ShowColors()
    {
        ShowColors(_graphView, _startNode, _goalNode);
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;

        while (!IsComplete)
        {
            if (_frontierNodes.Count > 0)
            {
                Node currentNode = _frontierNodes.Dequeue();
                _iterations++;

                if (!_exploredNodes.Contains(currentNode))
                {
                    _exploredNodes.Add(currentNode);
                }

                ExpandFrontier(currentNode);
                ShowColors();
                if (_graphView != null)
                {
                    _graphView.ShowNodeArrows(_frontierNodes.ToList());
                }

                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                IsComplete = transform;
            }
        }
    }

    void ExpandFrontier(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!_exploredNodes.Contains(node.neighbors[i]) 
                    && !_frontierNodes.Contains(node.neighbors[i]))
                {
                    node.neighbors[i].previous = node;
                    _frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }
}
