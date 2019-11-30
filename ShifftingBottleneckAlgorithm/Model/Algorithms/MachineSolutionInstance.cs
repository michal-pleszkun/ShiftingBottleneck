using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftingBottleneckAlgorithm.Model.Algorithms
{
    public class MachineSolutionInstance
    {
        public List<Node> nodesInOrder { get; set; } = new List<Node>();
        public int Lateness { get; set; }
    }
}
