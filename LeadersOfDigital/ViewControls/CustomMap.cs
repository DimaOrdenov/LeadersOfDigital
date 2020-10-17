using System;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.ViewControls
{
    public class CustomMap : Map
    {
        public ICommand PinClickedCommand { get; set; }
    }
}
