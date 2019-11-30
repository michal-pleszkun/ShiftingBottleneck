using System.Collections.Generic;

namespace ShiftingBottleneckAlgorithm.Model.Algorithms
{
    public class BellmanFordAlgorithm
    {
        private readonly int _edgesCount;
        private readonly int _verticesCount;

        private readonly Dictionary<Node, int> _distance = new Dictionary<Node, int>();
        private readonly Graph _graph;

        public BellmanFordAlgorithm(Graph graph)
        {
            _verticesCount = graph.NodesCount;
            _edgesCount = graph.Edges.Count;

            foreach (var node in graph.AllNodes)
            {
                _distance.Add(node, -1);
            }
            _graph = graph;
        }

        public Dictionary<Node,int> BellmanFord(Node source)
        {
            _distance[source] = 0;

            for (var i = 0; i <= _verticesCount -1; i++)
            {
                for (var j = 0; j < _edgesCount; j++)
                {
                    var u = _graph.Edges[j].StartNode;
                    var v = _graph.Edges[j].EndNode;
                    var weight = _graph.Edges[j].Weight;

                    if (_distance[u] != -1 && _distance[u] + weight < _distance[v]) continue;
                    {
                        _distance[v] = _distance[u] + weight;
                    }
                }
            }
            return _distance;
        }
    }
}