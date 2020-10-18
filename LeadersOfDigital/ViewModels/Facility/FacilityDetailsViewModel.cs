using LeadersOfDigital.BusinessLayer;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeadersOfDigital.ViewModels.Facility
{
    public class FacilityDetailsViewModel : PageViewModel
    {
        private readonly IFacilitiesLogic _facilitiesLogic;
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

        public override async void Prepare<TParameter>(TParameter parameter)
        {
            base.Prepare(parameter);
            if (parameter is string id)
            {
                var response = await _facilitiesLogic.Get(id, CancellationToken);
                Name = response.Name;
                Address = response.Street + response.Number;
            }
        }
    }
}
