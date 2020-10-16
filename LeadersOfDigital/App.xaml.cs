using Autofac;
using LeadersOfDigital.Containers;
using LeadersOfDigital.Views;
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

        protected override void OnStart()
        {
            INavigationService navigationService = IocContainer.Container.Resolve<INavigationService>();

            navigationService.SetRootPage<MainPage>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
