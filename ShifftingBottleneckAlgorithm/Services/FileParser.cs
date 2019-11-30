using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShiftingBottleneckAlgorithm.Model;
using ShiftingBottleneckAlgorithm.Services.Interfaces;

namespace ShiftingBottleneckAlgorithm.Services
{
    public class FileParser : IParser
    {
        public JobShopInstance ParseFileToGraph(string filePath)
        {
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            var instance = new JobShopInstance();
            var jobAmount = GetJobsAmount(lines[0]);
            instance.Jobs = GetJobs(lines.Skip(1).Take(jobAmount).ToList());
            GetJobsValues(lines.Skip(jobAmount + 1).ToList(), instance);
            instance.GenerateGraph();
            return instance;
        }

        private void GetJobsValues(List<string> jobs, JobShopInstance instance)
        {
            var jobsInFile = new List<List<string>>();
            foreach (var j in jobs)
            {
                jobsInFile.Add(j.Split(' ').ToList());
            }

            foreach (var jobTask in instance.Jobs.SelectMany(j => j.JobTasks))
            {
                try
                {
                    var jobInFile = jobsInFile.FirstOrDefault(jif => jif[0] == jobTask.TaskName);
                    if (jobInFile == null) throw new ArgumentException("There is no Job definition in file for jobName : " + jobTask.TaskName);
                    jobTask.MachineName = jobInFile[2];
                    jobTask.ExecutionTime = int.Parse(jobInFile[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot Parse Values for job task : " + jobTask.TaskName);
                    throw;
                }
            }
        }

        private List<Job> GetJobs(List<string> jobs)
        {
            var parsedJobs = new List<Job>();
            for(var i = 0; i< jobs.Count();i++)
            {
                var newJob = new Job();
                newJob.JobId = i;
                var split = jobs[i].Split(' ');
                for (var index = 0; index < split.Length; index++)
                {
                    var jobName = split[index];
                    newJob.JobTasks.Add(new JobTask {TaskName = jobName, SortOrder = index,JobId = i});
                }
                parsedJobs.Add(newJob);
            }
            return parsedJobs;
        }

        private int GetJobsAmount(string line)
        {
            if(int.TryParse(line,out int number))
            {
                return number;
            }
            throw new ArgumentException();
        }
    }
}