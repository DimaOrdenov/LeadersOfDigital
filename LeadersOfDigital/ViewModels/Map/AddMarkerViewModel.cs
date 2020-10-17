using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.BusinessLayer;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LeadersOfDigital.ViewModels.Map
{
    public class AddMarkerViewModel : PopupPageViewModel
    {
        private ObservableCollection<ImageSource> _photosCollection;
        private string _selectedBarrier;
        private string _selectedReason;
        private readonly IBarriersLogic _barriersLogic;

        public ICommand AddPhotoCommand { get; }

        public ICommand RegisterMarker { get; }

        public AddMarkerViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IBarriersLogic barriersLogic)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _barriersLogic = barriersLogic;

            AddPhotoCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await Task.Delay(1000);

                    PhotosCollection.Add(PhotosCollection.Count % 2 == 0 ? AppImages.ImageBarrier1 : AppImages.ImageBarrier2);

                    State = PageStateType.Default;
                });

            RegisterMarker = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(
                            async () =>
                            {

                            }));

                    State = PageStateType.Default;
                });

            _photosCollection = new ObservableCollection<ImageSource>();
        }

        public ObservableCollection<ImageSource> PhotosCollection
        {
            get => _photosCollection;
            set => SetProperty(ref _photosCollection, value);
        }

        public string SelectedBarrier
        {
            get => _selectedBarrier;
            set => SetProperty(ref _selectedBarrier, value);
        }

        public string SelectedReason
        {
            get => _selectedReason;
            set => SetProperty(ref _selectedBarrier, value);
        }
    }
}
