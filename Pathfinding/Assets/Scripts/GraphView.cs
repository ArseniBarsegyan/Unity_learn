using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] NodeViews;

    public Color BaseColor = Color.white;
    public Color WallColor = Color.black;

    void Start()
    {
    }

    void Update()
    {
    }

    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("WARNING! No graph to initialize!");
            return;
        }
        NodeViews = new NodeView[graph.Width,graph.Height];

        foreach (var node in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            var nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Init(node);
                NodeViews[node.xIndex, node.yIndex] = nodeView;

                if (node.NodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(WallColor);
                }
                else
                {
                    nodeView.ColorNode(BaseColor);
                }
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (var node in nodes)
        {
            if (node != null)
            {
                var nodeView = NodeViews[node.xIndex, node.yIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    public void ShowNodeArrows(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = NodeViews[node.xIndex, node.yIndex];
            if (nodeView != null)
            {
                nodeView.ShowArrow(color);
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes, Color color)
    {
        foreach (var node in nodes)
        {
            ShowNodeArrows(node, color);
        }
    }
}
