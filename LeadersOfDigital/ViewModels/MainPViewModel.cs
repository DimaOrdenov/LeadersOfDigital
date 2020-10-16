using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace LeadersOfDigital.ViewModels
{
    public class MainPViewModel : PageViewModel
    {
        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand ShowMeCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            ZoomInCommand = BuildPageVmCommand(() =>
            {
                MainMap.MoveToRegion(MainMap.VisibleRegion.WithZoom(2));

                return Task.CompletedTask;
            });

            ZoomOutCommand = BuildPageVmCommand(() =>
            {
                MainMap.MoveToRegion(MainMap.VisibleRegion.WithZoom(0.5));

                return Task.CompletedTask;
            });

            ShowMeCommand = BuildPageVmCommand(async () =>
            {
                State = PageStateType.MinorLoading;

                await TryMoveToUserLocation();

                State = PageStateType.Default;
            });

            MainMap = new Xamarin.Forms.GoogleMaps.Map
            {
                IsIndoorEnabled = true,
            };

            MainMap.MoveCamera(CameraUpdateFactory.NewPosition(new Position(55.751314, 37.627335)));
        }

        public Xamarin.Forms.GoogleMaps.Map MainMap { get; }

        public override async Task OnAppearing()
        {
            if (PageDidAppear)
            {
                return;
            }

            State = PageStateType.MinorLoading;

            await TryMoveToUserLocation();

            State = PageStateType.Default;

            await base.OnAppearing();
        }

        private async Task<bool> TryMoveToUserLocation()
        {
            Location locationToMove = null;

            Location defaultLocation = new Location(55.751314, 37.627335);

            if (await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.LocationWhenInUse()) != PermissionStatus.Granted &&
                await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.LocationAlways()) != PermissionStatus.Granted)
            {
                locationToMove = defaultLocation;

                MoveToPosition(new Position(locationToMove.Latitude, locationToMove.Longitude));

                return false;
            }

            MainMap.IsShowingUser = true;

            try
            {
                locationToMove = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            }
            catch (Exception e)
            {
                DebuggerService.Log(e);
            }

            if (locationToMove == null)
            {
                try
                {
                    locationToMove = await Geolocation.GetLastKnownLocationAsync();
                }
                catch (Exception e)
                {
                    DebuggerService.Log(e);
                }
            }

            if (locationToMove == null)
            {
                locationToMove = defaultLocation;

                MoveToPosition(new Position(locationToMove.Latitude, locationToMove.Longitude));

                return false;
            }

            MoveToPosition(new Position(locationToMove.Latitude, locationToMove.Longitude));

            return true;
        }

        private void MoveToPosition(Position position) =>
            MainMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(6)));
    }
}
