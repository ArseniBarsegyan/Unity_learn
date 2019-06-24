using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;

    public Color BaseColor = Color.white;
    public Color WallColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

        foreach (var node in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            var nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Init(node);

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
}
