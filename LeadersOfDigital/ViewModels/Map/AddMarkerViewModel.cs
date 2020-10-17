using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using DataModels.Requests;
using DataModels.Responses;
using DataModels.Responses.Enums;
using LeadersOfDigital.BusinessLayer;
using LeadersOfDigital.Definitions.VmLink;
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
        private readonly IBarriersLogic _barriersLogic;
        private readonly IFacilitiesLogic _facilitiesLogic;

        private ImageSource _photo;
        private string _selectedBarrier;
        private string _selectedReason;
        private string _comment;

        public ICommand AddPhotoCommand { get; }

        public ICommand RegisterMarkerCommand { get; }

        public AddMarkerViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IBarriersLogic barriersLogic,
            IFacilitiesLogic facilitiesLogic)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _barriersLogic = barriersLogic;
            _facilitiesLogic = facilitiesLogic;

            AddPhotoCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await Task.Delay(1000);

                    Photo = AppImages.ImageBarrier1;

                    State = PageStateType.Default;
                });

            RegisterMarkerCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(
                            async () =>
                            {
                                FacilityResponse facility = await _facilitiesLogic.AddFacility(
                                    new FacilityRequest
                                    {
                                        Street = AddMarkerVmLink.Address,
                                        Latitude = AddMarkerVmLink.Latitude,
                                        Longitute = AddMarkerVmLink.Longitute,
                                    },
                                    CancellationToken);

                                byte[] photo = null;

                                if (_photo is StreamImageSource streamImageSource)
                                {
                                    Stream stream = await streamImageSource.Stream(CancellationToken);

                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        await stream.CopyToAsync(memoryStream);

                                        photo = memoryStream.ToArray();
                                    }
                                }

                                await _barriersLogic.Post(
                                    new BarrierRequest
                                    {
                                        BarrierType = BarrierType.Ladder,
                                        Facility = new FacilityRequest { Id = facility.FacilityId },
                                        Photo = photo,
                                        Comment = _comment,
                                    },
                                    CancellationToken);

                                await DialogService.DisplayAlert("Спасибо!", "Маркер добавлен!", "Ок");

                                await NavigationService.NavigateBackAsync();
                            })
                        .OnFail(
                            ex =>
                            {
                                DialogService.ShowPlatformShortAlert($"Ошибка: {ex.Message}");

                                return Task.CompletedTask;
                            }));

                    State = PageStateType.Default;
                });
        }

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                SetProperty(ref _photo, value);

                OnPropertyChanged(nameof(HasPhoto));
            }
        }

        public bool HasPhoto => Photo != null;

        public string SelectedBarrier
        {
            get => _selectedBarrier;
            set => SetProperty(ref _selectedBarrier, value);
        }

        public string SelectedReason
        {
            get => _selectedReason;
            set => SetProperty(ref _selectedReason, value);
        }

        public string Comment { get; set; }

        public AddMarkerVmLink AddMarkerVmLink { get; private set; }

        public override void Prepare<TParameter>(TParameter parameter)
        {
            base.Prepare(parameter);

            if (parameter is AddMarkerVmLink vmLink)
            {
                AddMarkerVmLink = vmLink;
            }
        }
    }
}
