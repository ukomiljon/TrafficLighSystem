using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class PeakHours : IRule
    {
        [JsonProperty]
        private TimeSpan _start;
        [JsonProperty]
        private TimeSpan _end;
        [JsonProperty]
        private int _millisecondsWait;

        public PeakHours(TimeSpan start, TimeSpan end, int millisecondsWait)
        {
            _start = start;
            _end = end;
            _millisecondsWait = millisecondsWait;
        }

        public Task Perform(int stayedTime = 0) =>
            this.IsAccepted() ?
                Task.Run(() => Thread.Sleep(_millisecondsWait - stayedTime)) : null;


        public bool IsAccepted() =>
            _start <= DateTime.Now.TimeOfDay
             && _end > DateTime.Now.TimeOfDay;

        public int GetStayedTime() => _millisecondsWait;
        
    }
}
