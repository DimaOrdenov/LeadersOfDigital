using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.BusinessLayer;
using LeadersOfDigital.ViewModels.Common;
using LeadersOfDigital.Views.VolunteerAccount;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LeadersOfDigital.ViewModels.VolunteerAccount
{
    public class VolounteerRegistrationViewModel : PopupPageViewModel
    {
        private readonly ICommand _sexMenuItemTapCommand;

        private ImageSource _photo;
        private string _firstName;
        private string _secondName;
        private string _lastName;
        private DateTime _birthday;

        public ICommand UploadPhotoCommand { get; }

        public ICommand BirthdateSelectedCommand { get; }

        public ICommand RegisterCommand { get; }

        public VolounteerRegistrationViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext extendedUserContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            UploadPhotoCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await Task.Delay(1000);

                    Photo = AppImages.ImageVolounteer;

                    State = PageStateType.Default;
                });

            BirthdateSelectedCommand = BuildPageVmCommand<DateTime>(
                datetime =>
                {
                    _birthday = datetime;

                    return Task.CompletedTask;
                });

            RegisterCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await Task.Delay(1000);

                    extendedUserContext.SetContext(FirstName, SecondName, LastName);

                    await DialogService.DisplayAlert("Ура!", "Вы успешно зарегистрировались!", "Ок");

                    await NavigationService.NavigateBackAsync(false);

                    await NavigationService.NavigatePopupAsync<VolounteerAccountPage>();

                    State = PageStateType.Default;
                });

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

        public bool IsPhotoChosen => _photo != null;

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                SetProperty(ref _photo, value);

                OnPropertyChanged(nameof(IsPhotoChosen));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string SecondName
        {
            get => _secondName;
            set => SetProperty(ref _secondName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public IEnumerable<FloatingMenuItem> SexMenuItemsCollection { get; }
    }
}
