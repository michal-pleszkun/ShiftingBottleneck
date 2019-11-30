using System;
using System.Collections.Generic;
using System.Linq;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class Graph
    {
        public Node StartNode { get; set; } = new Node{ Name = "Start" };
        public Node EndNode { get; set; } = new Node { Name = "End"};
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int NodesCount => AllNodes.Count() + 1; // +1 end node :)

        public List<Node> AllNodes
        {
            get
            {
                var allNodes = Edges.Select(e => e.StartNode).ToList();
                allNodes.Add(EndNode);
                return allNodes.Distinct().ToList();
            }
        }

        public Edge GetStartingEdgeForJob(int JobId)
        {
            return Edges.FirstOrDefault(ed => ed.JobId == JobId && ed.StartNode == StartNode);
        }

        public List<Edge> TakeOnlyNextEdges(Node node)
        {

            List<Edge> nextEdges = new List<Edge>();
            foreach (var edge in Edges.Where(edge => edge.StartNode == node))
            {
                nextEdges.Add(edge);
                nextEdges.AddRange(TakeOnlyNextEdges(edge.EndNode));
            }
            return nextEdges;
        }

        public void AddArcs(List<Node> valueSelectedNodes)
        {
            for (var index = 0; index < valueSelectedNodes.Count - 1; index++)
            {
                var node = valueSelectedNodes[index];
                Edges.Add(new Edge { StartNode = this.AllNodes.First(n => n.Name == node.Name), Weight = node.ExecutionTime, EndNode = this.AllNodes.First(n => n.Name == valueSelectedNodes[index + 1].Name) });
            }
        }
    }
}
