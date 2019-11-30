using System;
using System.Collections.Generic;
using System.Linq;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class JobShopInstance
    {
        public List<Job> Jobs { get; set; }
        public Graph Graph { get; set; }
        public int Makespan { get; set; }

        public List<String> GetMachines()
        {
            return Graph.Edges.Select(e => e.StartNode.Machine).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        }

        public void GenerateGraph()
        {
            this.Graph = new Graph();
            foreach (var job in Jobs)
            {
                for (var index = 0; index < job.JobTasks.Count; index++)
                {
                    var jobTask = job.JobTasks[index];
                    var edge = new Edge();
                    edge.StartNode = jobTask.SortOrder == 0 ? Graph.StartNode : Graph.Edges.Last().EndNode;
                    edge.EndNode = new Node { Machine = jobTask.MachineName,JobId = job.JobId,Name = jobTask.TaskName, ExecutionTime = jobTask.ExecutionTime};
                    if (jobTask.SortOrder == 0)
                    {
                        edge.Weight = 0;
                    }
                    else
                    {
                        edge.Weight = Graph.Edges.Last().EndNode.ExecutionTime;
                    }
                    Graph.Edges.Add(edge);
                    if (index == job.JobTasks.Count - 1)
                    {
                        var lastEdge = new Edge{ StartNode = Graph.Edges.Last().EndNode,EndNode = Graph.EndNode,JobId = job.JobId, Weight = Graph.Edges.Last().EndNode.ExecutionTime};
                        Graph.Edges.Add(lastEdge);
                    }
                }
            }
        }
    }
}
