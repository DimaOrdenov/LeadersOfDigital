using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(LeadersOfDigital.iOS.CustomRenderers.EntryRenderer))]
namespace LeadersOfDigital.iOS.CustomRenderers
{
    public class EntryRenderer : Xamarin.Forms.Platform.iOS.EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || Control == null)
            {
                return;
            }

            Control.BorderStyle = UITextBorderStyle.None;

            if (!(Element is Entry view))
            {
                return;
            }

            UIToolbar toolbar = new UIToolbar();
            toolbar.SizeToFit();

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
            };

            Control.InputAccessoryView = toolbar;
        }
    }
}
