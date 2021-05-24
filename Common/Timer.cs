using System;

namespace Common
{
    public class Timer
    {
        private System.Timers.Timer _timer;
        private int _count = 0;
        public Timer()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.Write("{0,3}\b\b\b", ++_count);
        }

        public int GetCount() => _count;

        public void Restart()
        {
            _count = 0;
        }

        public int GetTime()
        {
            return _count;
        }
    }
}
