using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LeadersOfDigital.Views.Map
{
    public partial class AddMarkerPage : BasePopupPage
    {
        public AddMarkerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            addressFieldIcon.SetTintColor(AppColors.Secondary);
        }
    }
}
