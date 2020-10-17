using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.Definitions.Requests
{
    public class GoogleApiDirectionsRequest
    {
        /// <summary>
        /// driving, walking, transit
        /// </summary>
        public string TravelMode { get; set; }

        public Position Origin { get; set; }

        public Position Destination { get; set; }
    }
}
