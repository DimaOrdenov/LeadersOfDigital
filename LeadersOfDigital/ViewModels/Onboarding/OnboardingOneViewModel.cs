using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.ViewModels.Common;
using LeadersOfDigital.Views.Onboarding;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LeadersOfDigital.ViewModels.Onboarding
{
    public class OnboardingOneViewModel : PageViewModel
    {
        private int _step = 1;
        private readonly ICommand _sexMenuItemTapCommand;

        public ICommand PreviousStepCommand { get; }

        public ICommand NextStepCommand { get; }

        public ICommand RegisterCommand { get; }

        public ICommand PassRegistrationCommand { get; }

        public OnboardingOneViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            NextStepCommand = BuildPageVmCommand(
                () =>
                {
                    Step += 1;

                    return Task.CompletedTask;
                });

            PreviousStepCommand = BuildPageVmCommand(
                () =>
                {
                    Step -= 1;

                    return Task.CompletedTask;
                });

            RegisterCommand = BuildPageVmCommand(
                async () =>
                {
                    await DialogService.DisplayAlert("Ура!", "Вы успешно зарегистрированы!", "Ок");

                    await NavigationService.NavigateAsync<OnboardingTwoPage>();
                });

            PassRegistrationCommand = BuildPageVmCommand(
                () => NavigationService.NavigateAsync<OnboardingTwoPage>());

            _sexMenuItemTapCommand = BuildPageVmCommand<FloatingMenuItem>(
                item =>
                {
                    if (item.IsActive)
                    {
                        return Task.CompletedTask;
                    }

                    if (SexMenuItemsCollection.FirstOrDefault(x => x.IsActive) is FloatingMenuItem activeItem)
                    {
                        activeItem.IsActive = false;
                    }

                    item.IsActive = true;

                    return Task.CompletedTask;
                });

            CarouselItems = new List<Tuple<string, ImageSource>>
            {
                new Tuple<string, ImageSource>("С нарушениями зрения", AppImages.ImageBlindPeople),
                new Tuple<string, ImageSource>("С нарушениями слуха", AppImages.ImageDeafPeople),
                new Tuple<string, ImageSource>("Нарушения опорно-двигательного аппарата", AppImages.ImageDisabledPeople),
                new Tuple<string, ImageSource>("Передвигающиеся на креслах-колясках", AppImages.ImageWheelchairPeople),
            };

            SexMenuItemsCollection = new List<FloatingMenuItem>
            {
                new FloatingMenuItem("Мужской")
                {
                    TapCommand = _sexMenuItemTapCommand,
                },
                new FloatingMenuItem("Женский")
                {
                    TapCommand = _sexMenuItemTapCommand,
                },
            };
        }

        public int Step
        {
            get => _step;
            set
            {
                SetProperty(ref _step, value);

                OnPropertyChanged(nameof(CanReturn));
            }
        }

        public bool CanReturn => _step == 2 || _step == 3;

        public IEnumerable<Tuple<string, ImageSource>> CarouselItems { get; }

        public IEnumerable<FloatingMenuItem> SexMenuItemsCollection { get; }
    }
}
