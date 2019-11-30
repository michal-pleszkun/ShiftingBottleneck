using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftingBottleneckAlgorithm.Services;

namespace ShiftingBottleneckAlgorithm.Model.Algorithms
{
    public class ShiftingBottleneckHeuristicAlgorithm
    {
        private LatenessService _latenessService = new LatenessService();

        public void Run(JobShopInstance instance)
        {
            var result = new List<Edge>();
            Dictionary<string, PartialBranchAndBoundResult> selectedMachines = new Dictionary<string, PartialBranchAndBoundResult>();
            var i = 0;
            while (true)
            {
                GenerateParametersForNodes(instance);
                var machines = instance.GetMachines();
                if (selectedMachines.Count == machines.Count) break;//exit
                Dictionary<string, PartialBranchAndBoundResult> minimalLatenessPerMachine = new Dictionary<string, PartialBranchAndBoundResult>();
                machines.RemoveAll(m => selectedMachines.ContainsKey(m));
                foreach (var machine in machines)
                {
                    var jobsPerMachine = instance.Graph.AllNodes.Where(n => n.Machine == machine).ToList().DeepClone();
                    var partialResults = new List<PartialBranchAndBoundResult>();
                    var machineMinimumLateness = BranchAndBound(jobsPerMachine.DeepClone(),new List<Node>(),partialResults); // start BranchAndBound
                    minimalLatenessPerMachine.Add(machine, machineMinimumLateness);
                }
                var maxLateness = minimalLatenessPerMachine.Max(k => k.Value.MinimalPossibleLateness);
                var selectedMachine = minimalLatenessPerMachine.First(m => m.Value.MinimalPossibleLateness == maxLateness);
                selectedMachines.Add(selectedMachine.Key,selectedMachine.Value);
                instance.Graph.AddArcs(selectedMachine.Value.SelectedNodes);
                PrintJobInstance(instance, i++);
            }

            PrintSolutionForMachines(selectedMachines,instance);
        }

        // used prncipal that it will be always Acyclic Graph
        private void GenerateParametersForNodes(JobShopInstance instance)
        {
            var bellmanFord = new BellmanFordAlgorithm(instance.Graph);
            var maximumdistances = bellmanFord.BellmanFord(instance.Graph.StartNode);
            instance.Makespan = maximumdistances[instance.Graph.EndNode];

            foreach (var node in instance.Graph.AllNodes)
            {
                node.ReleaseTime = maximumdistances[node];
            }

            foreach (var node in instance.Graph.AllNodes)
            {
                var longestPath = new BellmanFordAlgorithm(new Graph { Edges = instance.Graph.TakeOnlyNextEdges(node), StartNode = node, EndNode = instance.Graph.EndNode }).BellmanFord(node).GetValueOrDefault(instance.Graph.EndNode);
                var dueDate = instance.Makespan - longestPath + node.ExecutionTime;
                node.DueDate = dueDate < 0 ? 0 : dueDate;
            }
        }

        public PartialBranchAndBoundResult BranchAndBound(List<Node> nodesNotAllocated, List<Node> nodesAllocated,List<PartialBranchAndBoundResult> partialResults)
        {

            for (int i = 0; i < nodesNotAllocated.Count; i++)
            {
                var newNodesNotAllocated = nodesNotAllocated.DeepClone();
                var newNodesAllocated = nodesAllocated.DeepClone();
                var nodeForAllocation = newNodesNotAllocated[i];
                newNodesAllocated.Add(nodeForAllocation);
                newNodesNotAllocated.Remove(nodeForAllocation);
                var currentLateness = _latenessService.CalculateLateness(newNodesNotAllocated.DeepClone(), newNodesAllocated.DeepClone());
                // so I need to calculate some objective function where 
                var currentResult = new PartialBranchAndBoundResult
                {
                    MinimalPossibleLateness = currentLateness,
                    NodesToBeAllocated = newNodesNotAllocated.DeepClone(),
                    SelectedNodes = newNodesAllocated.DeepClone()
                };
                partialResults.Add(currentResult);
                //select for another calculation
            }
            var minObjective = partialResults.Min(pr => pr.MinimalPossibleLateness);
            var nextApproach = partialResults.Where(pr => pr.MinimalPossibleLateness == minObjective && pr.NodesToBeAllocated.Count != 0 && pr.Calculated == false);
            if (nextApproach.Any())
            {
                var next = nextApproach.First();
                next.Calculated = true;
                partialResults.Add(BranchAndBound(next.NodesToBeAllocated.DeepClone(), next.SelectedNodes.DeepClone(), partialResults));
            }
            var FinalLateness = partialResults.Min(pr => pr.MinimalPossibleLateness);
            var Result = partialResults.Where(pr => pr.MinimalPossibleLateness == FinalLateness && pr.NodesToBeAllocated.Count == 0);
            return Result.First();
        }



        //To much Deep Copies 
        private Node GetNode(string name, JobShopInstance instance)
        {
            return instance.Graph.AllNodes.First(n => n.Name == name);
        }

        private void PrintJobInstance(JobShopInstance newInstance, int i)
        {
            Console.WriteLine("Graph For iteration : " + i);
            foreach (var edge in newInstance.Graph.Edges)
            {
                Console.WriteLine(edge.StartNode.Name + " => " + edge.Weight + " => " + edge.EndNode.Name);
            }
        }

        private void PrintSolutionForMachines(Dictionary<string, PartialBranchAndBoundResult> selectedMachines, JobShopInstance instance)
        {

            foreach (var (key, value) in selectedMachines)
            {
                Console.WriteLine(key);
                foreach (var line in value.SelectedNodes.Select(s => GetNode(s.Name, instance).Name + " : Release Time -  " + GetNode(s.Name, instance).ReleaseTime + " ; End Time : " + (GetNode(s.Name, instance).ReleaseTime + GetNode(s.Name, instance).ExecutionTime)).ToList())
                {
                    Console.WriteLine(line);
                }


            }
            SaveToFile(selectedMachines,instance);
        }

        private void SaveToFile(Dictionary<string, PartialBranchAndBoundResult> selectedMachines,
            JobShopInstance instance)
        {
            using StreamWriter outputFile = new StreamWriter("Result.txt");
            foreach (var (key, value) in selectedMachines)
            {
                outputFile.WriteLine(key);
                foreach (var line in value.SelectedNodes.Select(s => GetNode(s.Name, instance).Name + " : Release Time -  " + GetNode(s.Name, instance).ReleaseTime + " ; End Time : " + (GetNode(s.Name, instance).ReleaseTime + GetNode(s.Name, instance).ExecutionTime)).ToList())
                {
                    outputFile.WriteLine(line);
                }


            }
            outputFile.Flush();
        }
    }
}
