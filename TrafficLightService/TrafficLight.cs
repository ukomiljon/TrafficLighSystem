using Common;
using System.Collections.Generic;

namespace TrafficLightService
{
    public class TrafficLight : TrafficLightBase
    {
        public TrafficLight(Iterator<Signal> signals, string name)
        {
            _signals = signals;
            _name = name;
        }

        public override IEnumerable<(Signal, int)> GetSignal()
        {
            foreach (var item in _signals.Next())
            {
                yield return item;
            }
        }
    }
}
