using System;
using Android.Widget;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;

namespace LeadersOfDigital.Droid.DependencyServices
{
    public class PlatformAlertMessageService : IPlatformAlertMessageService
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}
