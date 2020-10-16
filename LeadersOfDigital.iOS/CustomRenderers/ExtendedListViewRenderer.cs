using System;
using System.ComponentModel;
using LeadersOfDigital.iOS.CustomRenderers;
using NoTryCatch.Xamarin.Portable.ViewControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace LeadersOfDigital.iOS.CustomRenderers
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || !(sender is ExtendedListView extendedListView))
            {
                return;
            }

            if (e.PropertyName == nameof(ListView.SelectedItem) || e.PropertyName == nameof(ExtendedListView.HasDefaultItemSelection))
            {
                foreach (var cell in Control.VisibleCells)
                {
                    cell.SelectionStyle = !extendedListView.HasDefaultItemSelection ?
                        UITableViewCellSelectionStyle.None :
                        UITableViewCellSelectionStyle.Default;
                }
            }
        }
    }
}
