using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using LeadersOfDigital.ViewControls;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(LeadersOfDigital.Droid.CustomRenderers.CustomMapRenderer))]
namespace LeadersOfDigital.Droid.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IOnMarkerClickListener
    {
        public CustomMapRenderer(Context context)
            : base(context)
        {
        }

        public bool OnMarkerClick(Marker marker)
        {
            if (Element is CustomMap customMap)
            {
                customMap.PinClickedCommand?.Execute(new Pin
                {
                    Label = marker.Title,
                    Address = marker.Snippet,
                    Position = new Position(marker.Position.Latitude, marker.Position.Longitude),
                });
            }

            return true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (NativeMap?.UiSettings != null)
            {
                NativeMap.UiSettings.MyLocationButtonEnabled = false;
                NativeMap.UiSettings.ZoomControlsEnabled = false;
            }
        }

        protected override void OnMapReady(GoogleMap nativeMap, Map map)
        {
            base.OnMapReady(nativeMap, map);

            NativeMap.SetInfoWindowAdapter(null);
            NativeMap.SetOnMarkerClickListener(this);
        }
    }
}
