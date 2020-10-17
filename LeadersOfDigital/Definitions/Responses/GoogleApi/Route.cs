using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeadersOfDigital.Definitions.Responses.GoogleApi
{
    public class Route
    {
        public Bounds Bounds { get; set; }

        public string Copyrights { get; set; }

        public IList<Leg> Legs { get; set; }

        [JsonProperty("overview_polyline")]
        public Polyline OverviewPolyline { get; set; }

        public string Summary { get; set; }

        public IList<object> Warnings { get; set; }

        [JsonProperty("waypoint_order")]
        public IList<object> WaypointOrder { get; set; }
    }
}
