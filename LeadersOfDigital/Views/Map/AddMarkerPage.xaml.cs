using System;
using System.Linq;
using DataModels.Responses.Enums;
using NoTryCatch.Core.Extensions;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LeadersOfDigital.Views.Map
{
    public partial class AddMarkerPage : BasePopupPage
    {
        public AddMarkerPage()
        {
            InitializeComponent();

            foreach (string barrierType in Enum.GetValues(typeof(BarrierType)).Cast<BarrierType>().Select(x => x.GetEnumDescription()))
            {
                barrierPicker.Items.Add(barrierType);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            addressFieldIcon.SetTintColor(AppColors.Secondary);
            barrierFieldIcon.SetTintColor(AppColors.Secondary);
            reasonFieldIcon.SetTintColor(AppColors.Secondary);
        }
    }
}
