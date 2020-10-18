using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.ViewControls
{
    public class CustomPin
    {
        public Position Position { get; set; }

        public string Label { get; set; }

        public string Address { get; set; }

        public Definitions.Enums.PinType Type { get; set; }
    }
}
