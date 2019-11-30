using System;
using System.Collections.Generic;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class PartialBranchAndBoundResult
    {
        public int MinimalPossibleLateness { get; set; }
        public List<Node> SelectedNodes { get; set; }
        public List<Node> NodesToBeAllocated { get; set; }
        public bool Calculated { get; set; } = false;
    }
}
