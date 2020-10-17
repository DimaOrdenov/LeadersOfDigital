using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeadersOfDigital.Definitions.Models.GoogleApi
{
    public class GoogleDirection
    {
        [JsonProperty("geocoded_waypoints")]
        public IList<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        public IList<Route> Routes { get; set; }

        public string Status { get; set; }
    }
}
