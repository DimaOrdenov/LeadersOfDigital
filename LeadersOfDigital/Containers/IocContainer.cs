using System;
using Autofac;
using LeadersOfDigital.ViewModels;
using LeadersOfDigital.Views;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;

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
            builder.RegisterInstance(platformAlertMessageServiceImplementation).As<IPlatformAlertMessageService>().SingleInstance();

            // ViewModels
            builder.RegisterType<MainPViewModel>().AsSelf();

            Container = builder.Build();

            IPageFactory pageFactory = Container.Resolve<IPageFactory>();

            // Pages
            pageFactory.Configure<MainPage, MainPViewModel>(() => Container.Resolve<MainPViewModel>());

        }
    }
}
