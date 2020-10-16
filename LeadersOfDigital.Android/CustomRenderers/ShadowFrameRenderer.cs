using Android.Content;
using LeadersOfDigital.Droid.CustomRenderers;
using LeadersOfDigital.ViewControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRenderer))]
namespace LeadersOfDigital.Droid.CustomRenderers
{
    public class ShadowFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public ShadowFrameRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is ShadowFrame shadowFrame))
            {
                return;
            }

            if (shadowFrame.HasShadow)
            {
                Elevation = 30.0f;
                TranslationZ = 0.0f;
                SetZ(30f);
            }
        }
    }
}
