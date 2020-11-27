using System.Windows.Input;
using LeadersOfDigital.Views.MasterDetails;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LeadersOfDigital.ViewModels.MasterDetails
{
    public class MasterDetailsMasterViewModel : MasterDetailMasterViewModel
    {
        public MasterDetailsMasterViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
        }

        public override ICommand GetMenuItemTapCommand() =>
            BuildPageVmCommand<MasterDetailMenuItemViewModel>(
                async item =>
                {
                    State = PageStateType.MinorLoading;

                    NavigationService.PresentMasterDetailsChildPage<MasterDetailsMainPage>(item.PageType);

                    State = PageStateType.Default;
                });
    }
}
