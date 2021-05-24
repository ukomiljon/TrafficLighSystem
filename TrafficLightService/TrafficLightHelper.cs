using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightService
{
    public class TrafficLightHelper
    {
        private Action<string, string> _notify;
        private IParentChildStayCalculator _parentChildStayCalculator;

        public TrafficLightHelper(Action<string, string> notifier=null, IParentChildStayCalculator parentChildStayCalculator=null)
        {
            _notify = notifier;
            _parentChildStayCalculator = parentChildStayCalculator;
        }

        public TrafficLightHelper(IParentChildStayCalculator parentChildStayCalculator)
        {           
            _parentChildStayCalculator = parentChildStayCalculator;
        }


        public void Run(TrafficLightSet setTrafficLight, CancelEventArgs cancelEventArgs = null)
        {
            while (true)
            {
                foreach (var (mergedStayTimes, signals) in _parentChildStayCalculator.CalculateParentChildSignals(setTrafficLight))
                {
                    _notify("server", CreateMessage(signals));

                    Parallel.For(0, signals.Count, i =>
                    {
                        var pair = signals.ElementAt(i);
                        pair.Value.Perform(mergedStayTimes[i]);
                    });
                }

                if (cancelEventArgs != null && cancelEventArgs.Cancel) break;
            }
        }

        public string CreateMessage(Dictionary<string, Signal> signals)
        {
            var messages = new List<string>();
            var redLightsPos = DefineRedLight(signals.Values.ToList());
            var index = 0;
            foreach (var item in signals)
            {
                if (redLightsPos[index])
                    messages.Add($"{ item.Key } {item.Value.getParentStatus()}  {new RedLight().GetStatus()}");
                else
                    messages.Add($"{ item.Key } {item.Value.getParentStatus()}  {item.Value.GetStatus()}");

                index++;
            }

            return "\n" + string.Join("\n", messages);
        }

        public bool[] DefineRedLight(List<Signal> signals)
        {
            var redLights = new bool[signals.Count];
            for (int i = 0; i < signals.Count; i++)
            {
                var signal = signals[i];
                var isArrow = !(signal is GreenLight || signal is RedLight || signal is YellowLight);

                if (isArrow)
                {
                    if (i == 0) redLights[1] = true;
                    else if (i == 1) redLights[0] = true;
                    else if (i == 2) redLights[3] = true;
                    else if (i == 3) redLights[2] = true;
                }
            }

            return redLights;

        }

        public TrafficLightSet Build(TrafficLightDTOSet trafficLightDTOSet)
        {
            setParents(trafficLightDTOSet);

            return new TrafficLightSet() {
             new TrafficLight(
                new LightSequence(
                      trafficLightDTOSet[0].signals, trafficLightDTOSet[0].DefaultOnLight) ,trafficLightDTOSet[0].Name
                ),
             new TrafficLight(
                  new LightSequence(
                       trafficLightDTOSet[1].signals, trafficLightDTOSet[1].DefaultOnLight) ,trafficLightDTOSet[1].Name
                ),
              new TrafficLight(
               new LightSequence(
                     trafficLightDTOSet[2].signals, trafficLightDTOSet[2].DefaultOnLight) ,trafficLightDTOSet[2].Name
               ),
               new TrafficLight(
                  new LightSequence(
                       trafficLightDTOSet[3].signals, trafficLightDTOSet[3].DefaultOnLight) ,trafficLightDTOSet[3].Name
                )
            };
        }

       private void setParents(TrafficLightDTOSet trafficLightDTOSet)
        { 
            foreach (var items in trafficLightDTOSet)
            {
                foreach (var item in items.signals)
                {
                    var child = item.GetChild();
                    if (child != null) child.setParent(item);
                }
            }

        }

      
    } 
         
}
