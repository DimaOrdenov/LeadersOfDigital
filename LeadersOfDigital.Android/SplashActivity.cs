
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace LeadersOfDigital.Droid
{
    [Activity(
        Label = "Saferoute",
        Icon = "@mipmap/icon",
        RoundIcon = "@mipmap/icon_round",
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Task.Delay(1000).ContinueWith(t =>
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            });
        }
    }
}
