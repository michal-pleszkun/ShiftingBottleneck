using System;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class Node
    {
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Machine { get; set; }
        public int ExecutionTime { get; set; }
        public int ReleaseTime { get; set; }
        public int DueDate { get; set; }
    }
}
