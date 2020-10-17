using System;
using Newtonsoft.Json;

namespace LeadersOfDigital.Definitions.Models.GoogleApi
{
    public class Step
    {
        public Description Distance { get; set; }

        public Description Duration { get; set; }

        [JsonProperty("end_location")]
        public Position EndLocation { get; set; }

        [JsonProperty("html_instructions")]
        public string HtmlInstructions { get; set; }

        public Polyline Polyline { get; set; }

        [JsonProperty("start_location")]
        public Position StartLocation { get; set; }

        [JsonProperty("travel_mode")]
        public string TravelMode { get; set; }

        public string Maneuver { get; set; }
    }
}
