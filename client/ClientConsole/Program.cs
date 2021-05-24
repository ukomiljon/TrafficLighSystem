using Common;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace ClientConsole
{
    class Program
    {
        private static HubConnection _connection;
         
        static void Main(string[] args)
        {
            BuildConnection();
            CheckConnection();
            CreateTrafficLightInServer();
            RunTrafficLight();
            Console.ReadKey();
        }

        private static void BuildConnection()
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl(" http://localhost:5000/centralHub")
                    .Build();

            _connection.StartAsync().Wait();//connect to server 
        }

        private static void CheckConnection()
        {
            //check connect to server.
            _connection.InvokeCoreAsync("Connect", args: new[] { "Client", "Connect to Server." });
            _connection.On("ConnectionResponse", (string Sender, string Reposonse) =>
            {
                Console.WriteLine("Connected to server successfully.");
                Console.WriteLine();
            });
        }

        private static void CreateTrafficLightInServer()
        {
            // create initial traffic lights
            _connection.InvokeCoreAsync("CreateTrafficLights", args: new[]
                 {  "000666 Traffic Light Set", CreateTrafficLightSet() }
           );
            _connection.On("CreateTrafficLightsResponse", (string Sender, string Reposonse, bool error) =>
            {
                Console.WriteLine($"{Sender}: {Reposonse}");
                Console.WriteLine();
            });
        }

        private static void RunTrafficLight()
        {
            //run server
            _connection.InvokeCoreAsync("RunTrafficLights", args: new[] { "000666 Traffic Light Set", "run" });
            _connection.On("RunTrafficLightsResponse", (string sender, string state, bool error) =>
            {
                if (error)
                {
                    Console.WriteLine($"{sender}: {state}");
                    return;
                }

                Console.WriteLine($"{state}");

                timer.Restart();
            });
        }

        private static string CreateTrafficLightSet()
        {            
            var trafficLighSet = SettingsBuilder.BuildTrafficLightWithPickHoursSetSubSignal();
            return Common.JsonSerializer.Serialize(trafficLighSet);
        } 

        private static Common.Timer timer = new Common.Timer();

    }
}
