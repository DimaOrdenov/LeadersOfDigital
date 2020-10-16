using System;
using Foundation;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using UIKit;

namespace LeadersOfDigital.iOS.DependencyServices
{
    public class PlatformAlertMessageService : IPlatformAlertMessageService
    {
        private const double LONG_DELAY = 3.5;
        private const double SHORT_DELAY = 1.5;

        private NSTimer _alertDelay;
        private UIAlertController _alert;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        private void ShowAlert(string message, double seconds)
        {
            _alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) => DismissMessage());

            _alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(_alert, true, null);
        }

        private void DismissMessage()
        {
            _alert?.DismissViewController(true, null);
            _alertDelay?.Dispose();
        }
    }
}
