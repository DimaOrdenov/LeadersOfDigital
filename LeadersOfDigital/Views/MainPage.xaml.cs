using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LeadersOfDigital.Views
{
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();

            zoomInImage.SetTintColor(AppColors.Secondary);
            zoomOutImage.SetTintColor(AppColors.Secondary);
            showMeImage.SetTintColor(AppColors.Secondary);
            burgerMenuImage.SetTintColor(AppColors.Secondary);
        }
    }
}
