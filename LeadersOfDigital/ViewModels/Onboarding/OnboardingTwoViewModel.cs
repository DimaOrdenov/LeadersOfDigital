using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.Views;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LeadersOfDigital.ViewModels.Onboarding
{
    public class OnboardingTwoViewModel : PageViewModel
    {
        private int _step = 1;

        public ICommand PreviousStepCommand { get; }

        public ICommand NextStepCommand { get; }

        public OnboardingTwoViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            NextStepCommand = BuildPageVmCommand(
                async () =>
                {
                    if (_step != 5)
                    {
                        Step += 1;

                        return;
                    }

                    State = PageStateType.MinorLoading;

                    App.Current.Properties["IsFirstLaunch"] = false;
                    await App.Current.SavePropertiesAsync();

                    await NavigationService.NavigateAsync<MainPage>();

                    State = PageStateType.Default;
                });

            PreviousStepCommand = BuildPageVmCommand(
                () =>
                {
                    Step -= 1;

                    return Task.CompletedTask;
                });
        }

        public int Step
        {
            get => _step;
            set
            {
                SetProperty(ref _step, value);

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Image));
            }
        }

        public string Title
        {
            get
            {
                switch (_step)
                {
                    case 1:
                        return "Безбарьерный маршрут";
                    case 2:
                        return "Сообщить о препятствии";
                    case 3:
                        return "Помощь волонтера";
                    case 4:
                        return "Доступность транспорта";
                    default:
                        return "Загруженность объектов";
                }
            }
        }

        public string Description
        {
            get
            {
                switch (_step)
                {
                    case 1:
                        return "Стройте и выбирайте маршруты. Наши карты подскажут вам, если на вашем пути встретится какое-нибудь препятствие";
                    case 2:
                        return "При движении на маршруте вы можете сообщить о встретившимся препятствии, тем самым улучшить данный маршрут, помогая другим людям";
                    case 3:
                        return "Не всегда в наших силах добраться до маршрута самим, на помощь могут придти добрые люди - волонтеры. Просто нажмите на кнопку “Вызвать волонтера”";
                    case 4:
                        return "Если необходима поездка на автобусе, но вы не знаете, оборудован ли автобус низким полом, просто выберите остановку и посмотрите маршруты";
                    default:
                        return "Прежде чем посетить какое-то место посмотрите на его загруженность, а так же возможность посещения для маломобильных граждан";
                }
            }
        }

        public ImageSource Image
        {
            get
            {
                switch (_step)
                {
                    case 1:
                        return AppImages.ImageOnboarding21;
                    case 2:
                        return AppImages.ImageOnboarding22;
                    case 3:
                        return AppImages.ImageOnboarding23;
                    case 4:
                        return AppImages.ImageOnboarding24;
                    default:
                        return AppImages.ImageOnboarding25;
                }
            }
        }
    }
}
