using System;
using ShiftingBottleneckAlgorithm.Model.Algorithms;
using ShiftingBottleneckAlgorithm.Services;

namespace ShiftingBottleneckAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            FileParser fileParser = new FileParser();
            var instance = fileParser.ParseFileToGraph(@"input\source.txt");
            var algorithm = new ShiftingBottleneckHeuristicAlgorithm();
            algorithm.Run(instance);
            Console.WriteLine("Hello World!");
        }
    }
}
