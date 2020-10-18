using System;
using System.Collections.Generic;
using System.Linq;
using LeadersOfDigital.Definitions;
using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.ViewControls
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            CustomPins = new List<CustomPin>();
        }

        public event EventHandler<CustomPinClickedEventArgs> PinClickedEvent;

        public IList<CustomPin> CustomPins { get; }

        public void InvokePinClickedEvent(CustomPin pin) => PinClickedEvent?.Invoke(this, new CustomPinClickedEventArgs { Pin = pin });

        public void AddPin(CustomPin pin)
        {
            Pins.Add(new Pin
            {
                Label = pin.Label,
                Address = pin.Address,
                Position = pin.Position,
                Icon = pin.Icon,
            });

            CustomPins.Add(pin);
        }

        public void AddPins(IEnumerable<CustomPin> pins)
        {
            foreach (CustomPin pin in pins)
            {
                AddPin(pin);
            }
        }

        public void RemovePin(CustomPin pin)
        {
            Pin pinToDelete = Pins.FirstOrDefault(x => x.Position == pin.Position);

            if (pinToDelete != null)
            {
                Pins.Remove(pinToDelete);
            }

            CustomPins.Remove(pin);
        }

        public void RemovePins(IEnumerable<CustomPin> pins)
        {
            foreach (CustomPin pin in pins)
            {
                RemovePin(pin);
            }
        }
    }
}
