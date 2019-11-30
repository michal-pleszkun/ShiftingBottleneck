using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftingBottleneckAlgorithm.Model
{
    public class LatenessResult
    {
        public List<Edge> ResultGraph { get; set; }
        public int MaxLatenessResult { get; set; }
        public string Machine { get; set; }
    }
}
