using Common;
using System.Collections.Generic;

namespace TrafficLightService
{
    public abstract class TrafficLightBase
    {
        protected Iterator<Signal> _signals { get; set; }

        public abstract IEnumerable<(Signal, int)> GetSignal();
        public Iterator<Signal> GetSignals() => _signals;
        public string GetStatus() => $"{_signals.CurrentItem().GetStatus()}";
        protected string _name { get; set; }

        public (List<Signal>, int[]) GetSignalsWithStayTimes()
        {
            var signals = new List<Signal>();
            var stayTimes = new List<int>();
            foreach (var item in GetSignal())
            {
                var (signal, currentStayTime) = item;
                signals.Add(signal);
                stayTimes.Add(currentStayTime);
            }

            return (signals, stayTimes.ToArray());
        }

        public string GetName() => _name;
    }

    public class TrafficLightSet : List<TrafficLightBase> {
        public TrafficLightSet() { }
    }


}
