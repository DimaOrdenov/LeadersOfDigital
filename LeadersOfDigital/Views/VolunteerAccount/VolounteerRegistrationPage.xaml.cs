using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LeadersOfDigital.Views.VolunteerAccount
{
    public partial class VolounteerRegistrationPage : BasePopupPage
    {
        public VolounteerRegistrationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            photoPlaceholderImage.SetTintColor(AppColors.Secondary);
        }
    }
}
