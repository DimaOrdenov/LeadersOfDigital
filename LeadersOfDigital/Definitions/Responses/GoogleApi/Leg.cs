using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeadersOfDigital.Definitions.Models.GoogleApi
{
    public class Leg
    {
        public Description Distance { get; set; }

        public Description Duration { get; set; }

        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("end_location")]
        public Position EndLocation { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("start_location")]
        public Position StartLocation { get; set; }

        public IList<Step> Steps { get; set; }

        [JsonProperty("traffic_speed_entry")]
        public IList<object> TrafficSpeedEntry { get; set; }

        [JsonProperty("via_waypoint")]
        public IList<object> ViaWaypoint { get; set; }
    }
}
