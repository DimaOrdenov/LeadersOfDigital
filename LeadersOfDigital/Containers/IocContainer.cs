using Autofac;
using LeadersOfDigital.ViewModels;
using LeadersOfDigital.Views;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using RestSharp;
using LeadersOfDigital.BusinessLayer;

namespace LeadersOfDigital.Containers
{
    public static class IocContainer
    {
        public static IContainer Container { get; private set; }

        public static void Init(IPlatformAlertMessageService platformAlertMessageServiceImplementation)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Services
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<DebuggerService>().As<IDebuggerService>().SingleInstance();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<UserContext>().AsSelf().SingleInstance();
            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();


            // BL
            var RC = new RestClient("http://city-env.eba-j4m8mgch.us-east-2.elasticbeanstalk.com/api");
            builder.RegisterInstance<IRestClient>(RC);
            builder.RegisterInstance<UserContext>(new UserContext());
            builder.RegisterType<BarriersLogic>().As<IBarriersLogic>().SingleInstance();
            builder.RegisterType<DisabilitiesLogic>().As<IDisabilitiesLogic>().SingleInstance();
            builder.RegisterType<FacilitiesLogic>().As<IFacilitiesLogic>().SingleInstance();

            // ViewModels
            builder.RegisterType<MainPViewModel>().AsSelf();

            // BL services
            builder.Register(context => new GoogleMapsApiLogicService(new RestClient("https://maps.googleapis.com/maps/"), context.Resolve<UserContext>(), context.Resolve<IDebuggerService>()))
                .As<IGoogleMapsApiLogicService>()
                .SingleInstance();

            Container = builder.Build();

            IPageFactory pageFactory = Container.Resolve<IPageFactory>();

            // Pages
            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());

        }
    }
}
