using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LeadersOfDigital.Containers;
using LeadersOfDigital.Droid.DependencyServices;
using Plugin.CurrentActivity;
using Android.Content;
using Android.Speech;
using System;

namespace LeadersOfDigital.Droid
{
    [Activity(
        Label = "Saferoute",
        Icon = "@mipmap/icon",
        RoundIcon = "@mipmap/icon_round",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private IAndroidSpeechToTextService _speechToTextService;

        public static readonly string NotificationChannelId = CrossCurrentActivity.Current.AppContext.PackageName;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            Xamarin.Essentials.Platform.OnResume();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _speechToTextService = new PlatformSpeechToTextService();

            IocContainer.Init(
                new PlatformAlertMessageService(),
                _speechToTextService);

            // Init nugets
            XamEffects.Droid.Effects.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

            PanCardView.Droid.CardsViewRenderer.Preserve();

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 10 &&
                resultCode == Result.Ok)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                if (matches.Count != 0)
                {
                    string textInput = matches[0];

                    _speechToTextService.InvokeSpeechRecognitionEvent(textInput);

                    Console.WriteLine(textInput);
                }
                else
                {
                    Console.WriteLine("nothing");
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}