using System;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LeadersOfDigital.ViewModels.MasterDetails
{
    public class MasterDetailsMainViewModel : MasterDetailViewModel
    {
        public MasterDetailsMainViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
        }
    }
}
