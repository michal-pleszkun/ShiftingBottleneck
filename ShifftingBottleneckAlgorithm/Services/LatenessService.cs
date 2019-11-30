using System.Collections.Generic;
using System.Linq;
using ShiftingBottleneckAlgorithm.Model;

namespace ShiftingBottleneckAlgorithm.Services
{
    public class LatenessService
    {
        public int CalculateLateness(List<Node> nodes, List<Node> alreadySelected)
        {
            var currentCMax = CalculateCmaxForOneList(alreadySelected);
            var numberOfNodes = nodes.Count + alreadySelected.Count;
            while(alreadySelected.Count < numberOfNodes)
            {
                List<Node> possibleAllocations = nodes.Where(n => n.ReleaseTime <= currentCMax).ToList();

                if (possibleAllocations.Count == 0)
                {
                    //move makespan to earliest release time
                    currentCMax = nodes.Min(n => n.ReleaseTime);
                }
                else
                {
                    var minimalDueDate = possibleAllocations.Min(m => m.DueDate);
                    var selectedNode = possibleAllocations.First(d => d.DueDate == minimalDueDate);
                    alreadySelected.Add(selectedNode);
                    currentCMax += selectedNode.ExecutionTime;
                    nodes.Remove(selectedNode);
                }
            }

            var lateness = currentCMax - alreadySelected.Last().DueDate;
            return lateness < 0 ? 0: lateness ;
        }

        public int CalculateCmaxForOneList(List<Node> alreadySelected)
        {
            var cmax = 0;
            foreach (var node in alreadySelected)
            {
                if (cmax >= node.ReleaseTime)
                {
                    cmax += node.ExecutionTime;
                }
                else
                {
                    cmax = node.ReleaseTime; // we will wait
                }
            }
            return cmax;
        }
    }
}
