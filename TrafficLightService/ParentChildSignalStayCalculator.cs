using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficLightService
{
    public interface IParentChildStayCalculator
    {
        public IEnumerable<(int[], Dictionary<string, Signal>)> CalculateParentChildSignals(TrafficLightSet trafficLights);
    }

    public class ParentChildSignalStayCalculator: IParentChildStayCalculator
    {
        private (Dictionary<string, List<Signal>>, int[][]) GetAllSubSignals(TrafficLightSet trafficLights)
        {
            var result = new Dictionary<string, List<Signal>>();
            var stayTimes = new int[trafficLights.Count][];
            var index = 0;
            foreach (var trafficLight in trafficLights)
            {
                var (list, stayTime) = trafficLight.GetSignalsWithStayTimes();

                result[trafficLight.GetName()] = list;
                stayTimes[index++] = stayTime;
            }

            return (result, stayTimes);
        }

        // it calculate marged staying time of light for across sub signals
        // e.i. green, red and yellow lights may have children 
        // green ligt may have 50s stay time, but it may have 10s right arrow, 15s passanger green light, 5s and 10s others 
        //[
        //  [10,15,5,10,50], [2], [2,4,10], 
        //]
        // [2,2,2], because min 2 in a row
        // [4,4,4], because second min is 4
        // [4, 4, 4] 10-6
        // [...]
        //TODO: it needs to optimize and simplify it.
        public IEnumerable<(int[], Dictionary<string, Signal>)> CalculateParentChildSignals(TrafficLightSet trafficLights)
        {
            var (results, stayTimes) = GetAllSubSignals(trafficLights);
            var mergedStayTimes = new int[results.Count];
            var columnIndex = 0;
            var indexArr = new int[results.Count];

            while (true)
            {
                int min = int.MaxValue;
                int max = int.MinValue;
                for (int i = 0; i < stayTimes.Length; i++)
                {
                    var stayTime = stayTimes[i][indexArr[i]];
                    if (min > stayTime && stayTime > 0 && stayTimes[i].Length - 1 > indexArr[i]) min = stayTime;
                    if (max < stayTime) max = stayTime;
                    mergedStayTimes[i] = stayTime;
                }

                var selectedSignals = new Dictionary<string, Signal>();
                for (int i = 0; i < indexArr.Length; i++)
                {
                    var pair = results.ElementAt(i);
                    selectedSignals[pair.Key] = pair.Value[indexArr[i]];
                }

                if (min == int.MaxValue)
                {
                    for (int i = 0; i < mergedStayTimes.Length; i++)
                    {
                        mergedStayTimes[i] = max;
                    }

                    // clean if there is no any sub signals
                    if (stayTimes.All(item => item.Length == 1))
                        mergedStayTimes = new int[mergedStayTimes.Length];

                    yield return (mergedStayTimes, selectedSignals);
                    yield break;
                }

                for (int i = 0; i < mergedStayTimes.Length; i++)
                {
                    mergedStayTimes[i] = min;
                    var currentStayTimes = stayTimes[i];
                    if (currentStayTimes[indexArr[i]] > 0) currentStayTimes[indexArr[i]] -= min;
                    if (currentStayTimes[indexArr[i]] == 0 && currentStayTimes.Length - 1 > indexArr[i]) indexArr[i]++;
                    currentStayTimes[currentStayTimes.Length - 1] -= min;
                }

                columnIndex++;

                yield return (mergedStayTimes, selectedSignals);
            }
        }
    }
}
