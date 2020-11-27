using Autofac;
using LeadersOfDigital.Containers;
using LeadersOfDigital.Views;
using LeadersOfDigital.Views.MasterDetails;
using LeadersOfDigital.Views.Onboarding;
using NoTryCatch.Xamarin.Portable.Services;
using Xamarin.Forms;

namespace LeadersOfDigital
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage();
        }

        protected override async void OnStart()
        {
            INavigationService navigationService = IocContainer.Container.Resolve<INavigationService>();

            navigationService.SetRootMasterDetailPage<MasterDetailsMainPage>();

            //if (App.Current.Properties.ContainsKey("IsFirstLaunch") &&
            //    App.Current.Properties["IsFirstLaunch"] as bool? == false)
            //{
            //    navigationService.SetRootMasterDetailPage<MasterDetailsMainPage>();
            //}
            //else
            //{
            //    if (!App.Current.Properties.ContainsKey("IsFirstLaunch"))
            //    {
            //        App.Current.Properties.Add("IsFirstLaunch", true);
            //        await App.Current.SavePropertiesAsync();
            //    }

            //    navigationService.SetRootPage<OnboardingOnePage>();
            //}
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
