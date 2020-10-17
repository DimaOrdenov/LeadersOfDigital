using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LeadersOfDigital.Views
{
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zoomInImage.SetTintColor(AppColors.Secondary);
            zoomOutImage.SetTintColor(AppColors.Secondary);
            showMeImage.SetTintColor(AppColors.Secondary);
            micImage.SetTintColor(AppColors.Secondary);
            burgerMenuImage.SetTintColor(AppColors.Secondary);
            originImage.SetTintColor(AppColors.Main);
            destinationImage.SetTintColor(AppColors.Main);
            callVolunteerImage.SetTintColor(AppColors.Secondary);
        }
    }
}
