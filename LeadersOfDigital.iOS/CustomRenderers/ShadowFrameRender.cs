using CoreGraphics;
using LeadersOfDigital.ViewControls;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using LeadersOfDigital.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRender))]
namespace LeadersOfDigital.iOS.CustomRenderers
{
    public class ShadowFrameRender : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is ShadowFrame shadowFrame))
            {
                return;
            }

            DrawLayer();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
                e.PropertyName == ShadowFrame.ShadowBlurProperty.PropertyName ||
                e.PropertyName == ShadowFrame.ShadowSpreadProperty.PropertyName ||
                e.PropertyName == ShadowFrame.ShadowColorProperty.PropertyName ||
                e.PropertyName == ShadowFrame.ShadowOffsetProperty.PropertyName ||
                e.PropertyName == ShadowFrame.ShadowOpacityProperty.PropertyName)
            {
                DrawLayer();
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            DrawLayer();
        }

        private void DrawLayer()
        {
            if (!(Element is ShadowFrame shadowFrame))
            {
                return;
            }

            Layer.ShadowRadius = !shadowFrame.HasShadow ? 0 : shadowFrame.ShadowBlur / 2;
            Layer.ShadowOpacity = !shadowFrame.HasShadow ? 0 : shadowFrame.ShadowOpacity;
            Layer.ShadowColor = !shadowFrame.HasShadow ? new CGColor(0, 0, 0) : shadowFrame.ShadowColor.ToCGColor();
            Layer.ShadowOffset = !shadowFrame.HasShadow ? new CGSize(0, 0) : shadowFrame.ShadowOffset.ToSizeF();

            if (shadowFrame.ShadowSpread == 0f || !shadowFrame.HasShadow)
            {
                Layer.ShadowPath = null;
            }
            else
            {
                Layer.ShadowPath = UIBezierPath.FromRoundedRect(Layer.Bounds.Inset(-shadowFrame.ShadowSpread, -shadowFrame.ShadowSpread), 0).CGPath;
            }
        }
    }
}
