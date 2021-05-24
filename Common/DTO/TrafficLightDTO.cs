using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    public class TrafficLightDTOSet : List<TrafficLightDTO>
    {
        public TrafficLightDTOSet() { } 
    }

    public class TrafficLightDTO
    {
        public TrafficLightDTO() { }
        public string Name { get; set; }
        public Signals signals { get; set; }
        public int DefaultOnLight { get; set; }
    }
}
