using System.Diagnostics;

namespace adventofcode;

public abstract class ExecutionMeasure
{
    internal protected double MeasureExecutionTime<T>(Func<T> action)
    {
        var stopwatch = new Stopwatch();
        var actionTimes = new List<double>();
        for (int index = 0; index < 2; index++)
        {
            stopwatch.Start();
            action();
            stopwatch.Stop();
            actionTimes.Add(stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Reset();
        }

        return actionTimes.Sum() / actionTimes.Count;
    }
}
