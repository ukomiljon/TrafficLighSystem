using Common;
using System;

namespace Common
{
    public class SettingsBuilder
    {
        public static Rules _stayYellowSN = new Rules() { new NormalHours(5000) };
        public static Rules _stayRedSN = new Rules() { new NormalHours(4000) };
        public static Rules _stayGreenSN = new Rules() { new NormalHours(20000) };

        public static Rules _stayGreenRightArrowSN = new Rules() { new NormalHours(10000) };
        public static Rules _stayGreenMixedSN = new Rules(){
                new PeakHours(new TimeSpan(8,0,0), new TimeSpan(10, 0, 0), 40000),
                new PeakHours(new TimeSpan(17, 0, 0), new TimeSpan(19, 0, 0), 40000),
                  new NormalHours(20000),
               };

        public static Rules _stayYellowWE = new Rules() { new NormalHours(5000) };
        public static Rules _stayRedWE = new Rules() { new NormalHours(4000) };
        public static Rules _stayGreenWE = new Rules() { new NormalHours(20000) };
        public static Rules _stayGreenMixedWE = new Rules(){
                new PeakHours(new TimeSpan(8,0,0), new TimeSpan(10, 0, 0), 10000),
                new PeakHours(new TimeSpan(17, 0, 0), new TimeSpan(19, 0, 0), 10000),
                new NormalHours(20000)
               };

        // rules: green, yellow, red
        public static TrafficLightDTO BuildTrafficLightDTO(string name, int DefaultOnLight, params Rules[] rules)
        {
            return
                new TrafficLightDTO()
                {
                    Name = name,
                    signals = new Signals() {
                         new GreenLight(rules[0]),
                         new YellowLight(rules[1]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2])
                    },
                    DefaultOnLight = DefaultOnLight
                };
        }

        public static TrafficLightDTO BuildTrafficLightDTOWithSubSignal(string name, int DefaultOnLight, params Rules[] rules)
        {
            return
                new TrafficLightDTO()
                {
                    Name = name,
                    signals = new Signals() {
                         new GreenLight(rules[0], new GreenRightArrowLight(_stayGreenRightArrowSN)),
                         new YellowLight(rules[1]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2]),
                         new RedLight(rules[2])
                    },
                    DefaultOnLight = DefaultOnLight
                };
        }

        public static TrafficLightDTOSet BuildTrafficLightNormalHoursSet()
        {
            return new TrafficLightDTOSet()
            {
                BuildTrafficLightDTO("South Traffic Light",0,_stayGreenSN, _stayYellowSN, _stayRedSN ),
                 BuildTrafficLightDTO("North Traffic Light",0,_stayGreenSN, _stayYellowSN, _stayRedSN ),
                  BuildTrafficLightDTO("West Traffic Light",3,_stayGreenWE, _stayYellowWE, _stayRedWE ),
                   BuildTrafficLightDTO("East Traffic Light",3,_stayGreenWE, _stayYellowWE, _stayRedWE ),
            };
        }

        public static TrafficLightDTOSet BuildTrafficLightWithPickHoursSet()
        {
            return new TrafficLightDTOSet()
            {
                BuildTrafficLightDTO("South Traffic Light",0,_stayGreenMixedSN, _stayYellowSN, _stayRedSN ),
                 BuildTrafficLightDTO("North Traffic Light",0,_stayGreenMixedSN, _stayYellowSN, _stayRedSN ),
                  BuildTrafficLightDTO("West Traffic Light",3,_stayGreenMixedWE, _stayYellowWE, _stayRedWE ),
                   BuildTrafficLightDTO("East Traffic Light",3,_stayGreenMixedWE, _stayYellowWE, _stayRedWE ),
            };
        }

        public static TrafficLightDTOSet BuildTrafficLightWithPickHoursSetSubSignal()
        {
            return new TrafficLightDTOSet()
            {
                BuildTrafficLightDTO("South Traffic Light",0,_stayGreenMixedSN, _stayYellowSN, _stayRedSN ),               
                 BuildTrafficLightDTOWithSubSignal("North Traffic Light",0,_stayGreenMixedSN, _stayYellowSN, _stayRedSN ), // add right arrow light
                  BuildTrafficLightDTO("West Traffic Light",3,_stayGreenMixedWE, _stayYellowWE, _stayRedWE ),
                   BuildTrafficLightDTO("East Traffic Light",3,_stayGreenMixedWE, _stayYellowWE, _stayRedWE ),
            };
        } 
    }
}
