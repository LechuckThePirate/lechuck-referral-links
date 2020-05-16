#region using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace LeChuck.ReferralLinks.Lambda.Timer
{
    public class ProcessTimer
    {
        public List<(TimeSpan Elapsed, string Label)> Marks = new List<(TimeSpan Elapsed, string Label)>();
        private readonly Stopwatch _stopWatch;

        public ProcessTimer(bool startTimer = false)
        {
            _stopWatch = new Stopwatch();
            if (startTimer) _stopWatch.Start();
        }

        public void Mark(string label, Action action)
        {
            action.Invoke();
            Marks.Add((_stopWatch.Elapsed, label));
        }

        public void Start()
        {
            Marks = new List<(TimeSpan Elapsed, string Label)>();
            _stopWatch.Restart();
        }

        public TimeSpan Total()
        {
            return _stopWatch.Elapsed;
        }

        public void LogMarks()
        {
            Console.WriteLine("Initialization times");
            Marks.ForEach(m => Console.WriteLine($" - {m.Label}: {m.Elapsed}"));
            Console.WriteLine($"Total time: {Total()}");
        }
    }
}