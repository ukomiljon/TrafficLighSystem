
using Common;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficLightService;

namespace CentralHub
{
    public class CentralHub : Hub
    {
        
        private static ConcurrentDictionary<string, TrafficLightSet> trafficLightsSets = new ConcurrentDictionary<string, TrafficLightSet>();

        public async Task Connect(string sender, string message)
        {
            await Clients.All.SendAsync("ConnectionResponse", "Server", "Hi client!");
        }        

        public async Task RunTrafficLights(string trafficLightSetName, string command)
        {
            try
            {
                TrafficLightSet setOfTrafficLight = null;

                if (trafficLightsSets.TryGetValue(trafficLightSetName, out setOfTrafficLight))
                {
                    Console.WriteLine($"{trafficLightSetName} runs.");

                    new TrafficLightHelper(async (trafficLight, status) =>
                    await Clients.All.SendAsync("RunTrafficLightsResponse", trafficLight, status, false)
                    , new ParentChildSignalStayCalculator()
                    ).Run(setOfTrafficLight);
                }
                else
                    await Clients.All.SendAsync("RunTrafficLightsResponse", "Server", $"{trafficLightSetName} is no exist.", true);
            }
            catch (Exception exp)
            { 
                await Clients.All.SendAsync("RunTrafficLightsResponse", "server", exp.Message, true);
            }
           
        } 

        public async Task CreateTrafficLights(string trafficLightSetName, string state)
        {
            try
            {
                var trafficLightDTO = Common.JsonSerializer.Deserialize<TrafficLightDTOSet>(state);
                trafficLightsSets.TryAdd(trafficLightSetName, new TrafficLightHelper(new ParentChildSignalStayCalculator()).Build(trafficLightDTO));
                await Clients.All.SendAsync("CreateTrafficLightsResponse", "server", "traffic light created successfully.", false);
            }
            catch (Exception exp)
            {
                await Clients.All.SendAsync("CreateTrafficLightsResponse", "server", exp.Message, true);
            }
          
        }
    }

}