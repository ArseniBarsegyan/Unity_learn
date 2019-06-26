﻿using System.Collections;
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
    public Color arrowColor = new Color32(216, 216, 216, 255);
    public Color highligthColor = new Color(1f,1f,0.5f,1f);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    private int _iterations = 0;
    public bool IsComplete = false;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        Dijkstra = 1
    }

    public Mode mode = Mode.BreadthFirstSearch;

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
        _startNode.distanceTraveled = 0;
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

        if (_pathNodes != null && _pathNodes.Count > 0)
        {
            graphView.ColorNodes(_pathNodes, pathColor);
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
        float timeStart = Time.time;
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

                if (_frontierNodes.Contains(_goalNode))
                {
                    _pathNodes = GetPathNodes(_goalNode);
                    if (exitOnGoal)
                    {
                        IsComplete = true;
                        Debug.Log("PATHFINDER mode: " + mode + 
                                  "   path length = " + _goalNode.distanceTraveled);
                    }
                }

                if (mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }

                if (showIterations)
                {
                    ShowDiagnostics();

                    yield return new WaitForSeconds(timeStep);
                }
                
            }
            else
            {
                IsComplete = transform;
            }
        }
        ShowDiagnostics();
        Debug.Log("PATHFINDER SearchRoutine: elapse time: " + (Time.time - timeStart) + " seconds");
    }

    private void ShowDiagnostics()
    {
        if (showColors)
        {
            ShowColors();
        }

        if (_graphView != null && showArrows)
        {
            _graphView.ShowNodeArrows(_frontierNodes.ToList(), arrowColor);

            if (_frontierNodes.Contains(_goalNode))
            {
                _graphView.ShowNodeArrows(_pathNodes, highligthColor);
            }
        }
    }

    void ExpandFrontierBreadthFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!_exploredNodes.Contains(node.neighbors[i]) 
                    && !_frontierNodes.Contains(node.neighbors[i]))
                {
                    float distanceToNeigbor = _graph.GetNodeDistance(node,
                        node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeigbor + node.distanceTraveled;
                    node.neighbors[i].distanceTraveled = newDistanceTraveled;

                    node.neighbors[i].previous = node;
                    _frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!_exploredNodes.Contains(node.neighbors[i]))
                {
                    float distanceToNeigbor = _graph.GetNodeDistance(node, 
                        node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeigbor + node.distanceTraveled;

                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTraveled) ||
                        newDistanceTraveled < node.neighbors[i].distanceTraveled)
                    {
                        node.neighbors[i].previous = node;
                        node.neighbors[i].distanceTraveled = newDistanceTraveled;
                    }

                    if (!_frontierNodes.Contains(node.neighbors[i]))
                    {
                        _frontierNodes.Enqueue(node.neighbors[i]);
                    }
                }
            }
        }
    }

    List<Node> GetPathNodes(Node endNode)
    {
        var path = new List<Node>();
        if (endNode == null)
        {
            return path;
        }
        path.Add(endNode);

        var currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}
