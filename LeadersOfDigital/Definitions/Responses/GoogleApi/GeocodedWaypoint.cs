using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeadersOfDigital.Definitions.Responses.GoogleApi
{
    public class GeocodedWaypoint
    {
        [JsonProperty("geocoder_status")]
        public string GeocoderStatus { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        public IList<string> Types { get; set; }
    }
}
