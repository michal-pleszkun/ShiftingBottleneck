using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class Edge
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public int Weight { get; set; }
        public int JobId { get; set; }
    }
}
