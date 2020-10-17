using Xamarin.Forms;

namespace LeadersOfDigital.Behaviors
{
    public class FocusElementBehavior : Behavior<View>
    {
        public static readonly BindableProperty ElementToFocusProperty = BindableProperty.Create(
            nameof(ElementToFocus),
            typeof(VisualElement),
            typeof(FocusElementBehavior));

        public VisualElement ElementToFocus
        {
            get => (VisualElement)GetValue(ElementToFocusProperty);
            set => SetValue(ElementToFocusProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            XamEffects.Commands.SetTap(bindable, new Command(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ElementToFocus?.Focus();
                });
            }));

            XamEffects.TouchEffect.SetColor(bindable, AppColors.RippleEffectBlack);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);

            XamEffects.Commands.SetTap(bindable, null);
            XamEffects.TouchEffect.SetColor(bindable, Color.Default);

            ElementToFocus = null;
        }
    }
}
