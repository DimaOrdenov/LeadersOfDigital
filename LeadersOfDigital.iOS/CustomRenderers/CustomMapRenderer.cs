using System;
using System.ComponentModel;
using Google.Maps;
using LeadersOfDigital.ViewControls;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(LeadersOfDigital.iOS.CustomRenderers.CustomMapRenderer))]
namespace LeadersOfDigital.iOS.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (Control is MapView nativeMap)
            {
                nativeMap.TappedMarker = TappedMarker;
            }
        }

        private bool TappedMarker(MapView mapView, Marker marker)
        {
            if (Element is CustomMap customMap)
            {
                customMap.PinClickedCommand?.Execute(new Pin
                {
                    Position = new Position(marker.Position.Latitude, marker.Position.Longitude),
                    Label = marker.Title,
                });
            }

            return true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control is MapView mapView &&
                mapView.Settings != null)
            {
                mapView.Settings.MyLocationButton = false;
                mapView.Settings.CompassButton = false;
            }
        }
    }
}
