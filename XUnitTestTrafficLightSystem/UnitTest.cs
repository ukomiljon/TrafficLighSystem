using Common;
using System;
using System.Collections.Generic;
using TrafficLightService;
using Xunit;

namespace XUnitTestTrafficLightSystem
{
    public class UnitTest
    {
        // each unit test takes about 1 minutes, if you increase this, unit test may run longer
        public const int NumerOfSequence = 5;

        [Theory]
        [InlineData(
            new int[] { 0, 20, 5, 4, 20, 5 },
            new string[]{
         "\nSouth Traffic Light   GreenLight\nNorth Traffic Light   GreenLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
         "\nSouth Traffic Light   YellowLight\nNorth Traffic Light   YellowLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"     ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   GreenLight\nEast Traffic Light   GreenLight" ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   YellowLight\nEast Traffic Light   YellowLight"  ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"    ,
        })]
        // Please Node: first 2 sequence could be 1 second different some times
        // Run again when fialed.
        public void TestNormalHours(int[] expectedStayedTimes, string[] expectedStatus)
        {
            Initialize();
            if (isPickHours()) return;
            var trafficLighSet = SettingsBuilder.BuildTrafficLightNormalHoursSet();

            var helper = new TrafficLightHelper((trafficLight, status) => Notify(trafficLight, status));
            var trafficLightSet = helper.Build(trafficLighSet);
            helper.Run(trafficLightSet, _cancelEventArgs);

            Assert.Equal(_counts.ToArray(), expectedStayedTimes);
            Assert.Equal(_statusList.ToArray(), expectedStatus);
        }


        [Theory]
        [InlineData(
           new int[] { 0, 10, 10, 5, 4, 20 },
           new string[]{

          "\nSouth Traffic Light   RedLight\nNorth Traffic Light GreenLight  GreenRightArrowLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
          "\nSouth Traffic Light   GreenLight\nNorth Traffic Light   GreenLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"    ,
          "\nSouth Traffic Light   YellowLight\nNorth Traffic Light   YellowLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"    ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   GreenLight\nEast Traffic Light   GreenLight"    ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   YellowLight\nEast Traffic Light   YellowLight"

       })]
        // Please Node: first 2 sequence could be 1 second different some times
        // Run again when fialed.
        public void TestNormalHoursWithSubSignal(int[] expectedStayedTimes, string[] expectedStatus)
        {
            Initialize();
            if (isPickHours()) return;

            var trafficLighSet = SettingsBuilder.BuildTrafficLightWithPickHoursSetSubSignal();

            var helper = new TrafficLightHelper((trafficLight, status) => Notify(trafficLight, status));
            var trafficLightSet = helper.Build(trafficLighSet);
            helper.Run(trafficLightSet, _cancelEventArgs);

            Assert.Equal(_counts.ToArray(), expectedStayedTimes);
            Assert.Equal(_statusList.ToArray(), expectedStatus);
        }


        [Theory]
        [InlineData(
          new int[] { 0, 30, 10, 5, 4, 20 },
          new string[]{
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light GreenLight  GreenRightArrowLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
          "\nSouth Traffic Light   GreenLight\nNorth Traffic Light   GreenLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"    ,
          "\nSouth Traffic Light   YellowLight\nNorth Traffic Light   YellowLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"  ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   RedLight\nEast Traffic Light   RedLight"    ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   GreenLight\nEast Traffic Light   GreenLight"    ,
          "\nSouth Traffic Light   RedLight\nNorth Traffic Light   RedLight\nWest Traffic Light   YellowLight\nEast Traffic Light   YellowLight"
      })]

        // PickHours, you can change it in SettingsBuilder in Common 
        // Please Node: first 2 sequence could be 1 second different some times
        // Run again when fialed.
        public void TestNormalHoursWithSubSignalPickHours(int[] expectedStayedTimes, string[] expectedStatus)
        {
            Initialize();
            if (!isPickHours()) return;// if no pickhours, it doesnot run.
            var trafficLighSet = SettingsBuilder.BuildTrafficLightWithPickHoursSetSubSignal();

            var helper = new TrafficLightHelper((trafficLight, status) => Notify(trafficLight, status));
            var trafficLightSet = helper.Build(trafficLighSet);
            helper.Run(trafficLightSet, _cancelEventArgs);

            Assert.Equal(_counts.ToArray(), expectedStayedTimes);
            Assert.Equal(_statusList.ToArray(), expectedStatus);
        }

        public void Notify(string trafficLight, string status)
        {
            _statusList.Add(status);
            _counts.Add(timer.GetCount());

            timer.Restart();//reset timer

            if (_counts.Count > NumerOfSequence)
                _cancelEventArgs.Cancel = true; // break look in server
        }

        private void Initialize()
        {
            timer = new Timer();
            _counts = new List<int>();
            _statusList = new List<string>();
            _cancelEventArgs = new CancelEventArgs();
        }

        public bool isPickHours()
        {
            return (new TimeSpan(8, 0, 0) <= DateTime.Now.TimeOfDay
                   && new TimeSpan(10, 0, 0) > DateTime.Now.TimeOfDay)
                   ||
                   (new TimeSpan(17, 0, 0) <= DateTime.Now.TimeOfDay
                   && new TimeSpan(19, 0, 0) > DateTime.Now.TimeOfDay);
        }

        private static Timer timer = new Timer();
        private static List<int> _counts = new List<int>();
        private static List<string> _statusList = new List<string>();
        private static CancelEventArgs _cancelEventArgs = new CancelEventArgs();
    }
}
