using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ShiftingBottleneckAlgorithm.Model
{
    [Serializable]
    public class Job
    {
        public int JobId { get; set; }
        public List<JobTask> JobTasks { get; set; } = new List<JobTask>();
        public int ThroughputTime { get; set; }

        public void CalculateThroughputTime()
        {
            int time = 0;
            foreach (var jobTask in JobTasks)
            {
                time += jobTask.ExecutionTime;
            }
            ThroughputTime = time;
        }
    }

    [Serializable]
    public class JobTask
    {
        public int JobId { get; set; }
        public string TaskName { get; set; }
        public int ExecutionTime { get; set; }
        public string MachineName { get; set; }
        public int SortOrder { get; set; }
    }
}
