using Xamarin.Forms;

namespace LeadersOfDigital.ViewControls
{
    public class ShadowFrame : Frame
    {
        public ShadowFrame()
        {
            HasShadow = true;
            IsClippedToBounds = false;
        }

        public static readonly BindableProperty ShadowBlurProperty = BindableProperty.Create(
            nameof(ShadowBlur),
            typeof(float),
            typeof(Frame),
            defaultValue: 10f);

        public static readonly BindableProperty ShadowSpreadProperty = BindableProperty.Create(
            nameof(ShadowSpread),
            typeof(float),
            typeof(Frame),
            defaultValue: 0f);

        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(
            nameof(ShadowColor),
            typeof(Color),
            typeof(Frame),
            Color.FromHex("#26000000"));

        public static readonly BindableProperty ShadowOffsetProperty = BindableProperty.Create(
            nameof(ShadowOffset),
            typeof(Size),
            typeof(Frame),
            defaultValue: new Size(0f, 0f));

        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create(
            nameof(ShadowOpacity),
            typeof(float),
            typeof(Frame),
            defaultValue: 1f);

        public float ShadowBlur
        {
            get => (float)GetValue(ShadowBlurProperty);
            set => SetValue(ShadowBlurProperty, value);
        }

        public float ShadowSpread
        {
            get => (float)GetValue(ShadowSpreadProperty);
            set => SetValue(ShadowSpreadProperty, value);
        }

        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public Size ShadowOffset
        {
            get => (Size)GetValue(ShadowOffsetProperty);
            set => SetValue(ShadowOffsetProperty, value);
        }

        public float ShadowOpacity
        {
            get => (float)GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }
    }
}
