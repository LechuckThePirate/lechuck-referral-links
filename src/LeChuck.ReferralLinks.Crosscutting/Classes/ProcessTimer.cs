using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LeChuck.ReferralLinks.Crosscutting.Classes
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
            _stopWatch.Start();
        }

        public TimeSpan Total()
        {
            return _stopWatch.Elapsed;
        }
    }
}
