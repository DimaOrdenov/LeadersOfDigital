using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeadersOfDigital.Views.Facility
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacilityDetailsPage : BasePopupPage
    {
        public FacilityDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            star1.SetTintColor(AppColors.Main);
            star2.SetTintColor(AppColors.Main);
            star3.SetTintColor(AppColors.Main);
            star4.SetTintColor(AppColors.PaleGray);
            star5.SetTintColor(AppColors.PaleGray);

            star12.SetTintColor(AppColors.Main);
            star22.SetTintColor(AppColors.Main);
            star32.SetTintColor(AppColors.PaleGray);
            star42.SetTintColor(AppColors.PaleGray);
            star52.SetTintColor(AppColors.PaleGray);

            icBrakePage.SetTintColor(AppColors.Secondary);
            icEscalator.SetTintColor(AppColors.Secondary);
            icLift.SetTintColor(AppColors.Secondary);
        }
    }
}