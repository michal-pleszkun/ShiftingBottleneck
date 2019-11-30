using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftingBottleneckAlgorithm.Model
{
    public class GraphResult
    {
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int maxLateness { get; set; }
        public string MachineNAme { get; set; }
    }
}
