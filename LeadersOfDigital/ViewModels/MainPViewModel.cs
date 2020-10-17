using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.BusinessLayer;
using GoogleApi = LeadersOfDigital.Definitions.Models.GoogleApi;
using LeadersOfDigital.ViewControls;
using NoTryCatch.BL.Core.Exceptions;
using NoTryCatch.BL.Core.Enums;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.ViewModels
{
    public class MainPViewModel : PageViewModel
    {
        private readonly IGoogleMapsApiLogicService _googleMapsApiLogicService;

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand ShowMeCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IGoogleMapsApiLogicService googleMapsApiLogicService)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _googleMapsApiLogicService = googleMapsApiLogicService;

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

                (bool result, Position myPosition) = await TryGetUserLocation();

                if (result)
                {
                    MoveToPosition(myPosition);
                }

                State = PageStateType.Default;
            });

            MainMap = new CustomMap
            {
                IsIndoorEnabled = true,
                PinClickedCommand = BuildPageVmCommand<Pin>(async pin =>
                {
                    State = PageStateType.Loading;

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(async () =>
                        {
                            (bool result, Position myPosition) = await TryGetUserLocation();

                            if (!result)
                            {
                                throw new BusinessLogicException(LogicExceptionType.BadRequest, "Не удалось определить ваше местоположение");
                            }

                            GoogleApi.GoogleDirection googleDirection =
                                await _googleMapsApiLogicService.GetDirections(myPosition.Latitude, myPosition.Longitude, pin.Position.Latitude, pin.Position.Longitude, CancellationToken);
                        }));

                    State = PageStateType.Default;
                }),
            };

            MainMap.MapLongClicked += async (sender, e) =>
            {
                await DialogService.DisplayAlert(string.Empty, $"Добавить маркер в {e.Point.Latitude};{e.Point.Longitude}", "Да", "Нет");
            };
        }

        public CustomMap MainMap { get; }

        public override async Task OnAppearing()
        {
            if (PageDidAppear)
            {
                return;
            }

            State = PageStateType.MinorLoading;

            MainMap.MoveCamera(CameraUpdateFactory.NewPosition(new Position(55.751314, 37.627335)));

            (bool result, Position myPosition) = await TryGetUserLocation();

            if (result)
            {
                MoveToPosition(myPosition);
            }

            MainMap.Pins.Add(new Pin
            {
                Position = new Position(55.751314, 37.627335),
                Label = "test",
                Address = "address",
                //Icon = BitmapDescriptorFactory.FromBundle("ic_pin.png"),
            });

            State = PageStateType.Default;

            await base.OnAppearing();
        }

        private async Task<(bool, Position)> TryGetUserLocation()
        {
            if (await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.LocationWhenInUse()) != PermissionStatus.Granted &&
                await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(new Permissions.LocationAlways()) != PermissionStatus.Granted)
            {
                return (false, default(Position));
            }

            MainMap.MyLocationEnabled = true;
            MainMap.IsShowingUser = true;

            Location location = null;

            try
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            }
            catch (Exception e)
            {
                DebuggerService.Log(e);
            }

            if (location == null)
            {
                try
                {
                    location = await Geolocation.GetLastKnownLocationAsync();
                }
                catch (Exception e)
                {
                    DebuggerService.Log(e);
                }
            }

            Position position = location != null ? new Position(location.Latitude, location.Longitude) : default;

            return (position != null, position);
        }

        private void MoveToPosition(Position position) =>
            MainMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(6)));

        public IEnumerable<Position> Decode(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new Position(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5);
            }
        }
    }
}
