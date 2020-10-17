using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.Definitions.Requests
{
    public class GoogleApiDirectionsRequest
    {
        public GoogleApiDirectionsRequest()
        {
            TravelMode = "walking";
            Alternatives = true;
        }

        /// <summary>
        /// driving, walking, transit
        /// </summary>
        public string TravelMode { get; }

        public Position Origin { get; set; }

        public Position Destination { get; set; }

        public bool Alternatives { get; }
    }
}
