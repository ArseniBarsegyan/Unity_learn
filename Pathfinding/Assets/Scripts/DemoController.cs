using UnityEngine;

public class DemoController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;

    void Start()
    {
        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            var graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graph);
            }
        }
    }
}
