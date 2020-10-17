using System;
using System.Collections.Generic;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;
using Xamarin.Forms;

namespace LeadersOfDigital.Views.VolunteerAccount
{
    public partial class VolounteerAccountPage : BasePopupPage
    {
        public VolounteerAccountPage()
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
        }
    }
}
