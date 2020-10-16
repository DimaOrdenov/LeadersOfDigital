using System;
using LeadersOfDigital.iOS.CustomRenderers;
using NoTryCatch.Xamarin.Portable.ViewControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedNavigationPage), typeof(ExtendedNavigationPageRenderer))]
namespace LeadersOfDigital.iOS.CustomRenderers
{
    public class ExtendedNavigationPageRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationBar.StandardAppearance.ShadowColor = UIColor.Clear;
            NavigationBar.StandardAppearance.ShadowImage = new UIImage();
        }
    }
}
