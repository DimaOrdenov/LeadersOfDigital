using Autofac;
using LeadersOfDigital.ViewModels;
using LeadersOfDigital.Views;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using RestSharp;
using NoTryCatch.BL.Core;
using LeadersOfDigital.Views.VolunteerAccount;
using LeadersOfDigital.ViewModels.VolunteerAccount;
using LeadersOfDigital.BusinessLayer;
using LeadersOfDigital.Helpers;
using LeadersOfDigital.ViewModels.Map;
using LeadersOfDigital.Views.Map;
using System.Reflection;
using NoTryCatch.Xamarin.Portable.ViewModels;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.BL.Core.Exceptions;
using System.Threading.Tasks;
using System;
using LeadersOfDigital.Views.Onboarding;
using LeadersOfDigital.ViewModels.Onboarding;

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
            builder.RegisterType<ExtendedUserContext>().As<UserContext>().AsSelf().SingleInstance();
            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();

            // BL
            var RC = new RestClient(Secrets.ApiUrl);
            builder.RegisterInstance<IRestClient>(RC);
            builder.RegisterType<BarriersLogic>().As<IBarriersLogic>().SingleInstance();
            builder.RegisterType<DisabilitiesLogic>().As<IDisabilitiesLogic>().SingleInstance();
            builder.RegisterType<FacilitiesLogic>().As<IFacilitiesLogic>().SingleInstance();

            // ViewModels
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.FullName.Contains(nameof(ViewModels)))
                .OnActivated(e =>
                {
                    if (!(e.Instance is PageViewModel pageVm))
                    {
                        return;
                    }

                    pageVm.ExceptionHandler.AddExceptionTypeHandler<BusinessLogicException>((ex, action) => Container.Resolve<DialogService>().DisplayAlert("Ошибка", ex.Message, "Ок"));
                    pageVm.ExceptionHandler.AddExceptionTypeHandler<Exception>((ex, action) => Container.Resolve<DialogService>().DisplayAlert("Ошибка", ex.Message, "Ок"));
                })
                .AsSelf();

            // BL services
            builder.Register(context => new GoogleMapsApiLogicService(new RestClient("https://maps.googleapis.com/maps/"), context.Resolve<UserContext>(), context.Resolve<IDebuggerService>()))
                .As<IGoogleMapsApiLogicService>()
                .SingleInstance();

            Container = builder.Build();

            IPageFactory pageFactory = Container.Resolve<IPageFactory>();

            // Pages
            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());
            pageFactory.Configure<VolounteerAccountPage, VolounteerAccountViewModel>(() => Container.Resolve<VolounteerAccountViewModel>());
            pageFactory.Configure<VolounteerRegistrationPage, VolounteerRegistrationViewModel>(() => Container.Resolve<VolounteerRegistrationViewModel>());
            pageFactory.Configure<AddMarkerPage, AddMarkerViewModel>(() => Container.Resolve<AddMarkerViewModel>());
            pageFactory.Configure<OnboardingOnePage, OnboardingOneViewModel>(() => Container.Resolve<OnboardingOneViewModel>());
            pageFactory.Configure<OnboardingTwoPage, OnboardingTwoViewModel>(() => Container.Resolve<OnboardingTwoViewModel>());
        }
    }
}
