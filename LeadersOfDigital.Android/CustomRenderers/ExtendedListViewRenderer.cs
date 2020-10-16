using System.ComponentModel;
using Android.Content;
using LeadersOfDigital.Droid.CustomRenderers;
using NoTryCatch.Xamarin.Portable.ViewControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace LeadersOfDigital.Droid.CustomRenderers
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        public ExtendedListViewRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || !(Element is ExtendedListView extendedListView))
            {
                return;
            }

            Control?.SetSelector(!extendedListView.HasDefaultItemSelection ?
                Resource.Drawable.list_view_no_ripple :
                Android.Resource.Drawable.ListSelectorBackground);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || !(sender is ExtendedListView extendedListView))
            {
                return;
            }

            if (e.PropertyName == nameof(ListView.SelectedItem) || e.PropertyName == nameof(ExtendedListView.HasDefaultItemSelection))
            {
                Control?.SetSelector(!extendedListView.HasDefaultItemSelection ?
                    Resource.Drawable.list_view_no_ripple :
                    Android.Resource.Drawable.ListSelectorBackground);
            }
        }
    }
}
