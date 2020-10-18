using DataModels.Responses;
using LeadersOfDigital.BusinessLayer;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeadersOfDigital.ViewModels.Facility
{
    public class FacilityDetailsViewModel : PageViewModel
    {
        private readonly IFacilitiesLogic _facilitiesLogic;

        private int _facilityId;

        public ICommand ConfirmCommand { get; }

        public FacilityDetailsViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IFacilitiesLogic facilitiesLogic)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _facilitiesLogic = facilitiesLogic;
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public override async Task OnAppearing()
        {
            if (PageDidAppear)
            {
                return;
            }

            await ExceptionHandler.PerformCatchableTask(
                new ViewModelPerformableAction(
                    async () =>
                    {
                        FacilityResponse facilitiesResponse = await _facilitiesLogic.Get(_facilityId.ToString(), CancellationToken);

                        Name = facilitiesResponse.Name;
                        Address = facilitiesResponse.Street + facilitiesResponse.Number;
                    }));

            await base.OnAppearing();
        }

        public override async void Prepare<TParameter>(TParameter parameter)
        {
            base.Prepare(parameter);

            if (parameter is int id)
            {
                _facilityId = id;
            }
        }
    }
}
