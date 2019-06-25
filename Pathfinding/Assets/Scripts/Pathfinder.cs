using System.Collections.Generic;
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
    }
}
