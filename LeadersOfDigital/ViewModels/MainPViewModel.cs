using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadersOfDigital.BusinessLayer;
using GoogleApi = LeadersOfDigital.Definitions.Responses.GoogleApi;
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
using LeadersOfDigital.Definitions.Requests;
using LeadersOfDigital.ViewModels.VolunteerAccount;
using LeadersOfDigital.Views.VolunteerAccount;
using LeadersOfDigital.Views.Facility;

namespace LeadersOfDigital.ViewModels
{
    public class MainPViewModel : PageViewModel
    {
        private readonly IFacilitiesLogic _facilitiesLogic;
        private readonly IGoogleMapsApiLogicService _googleMapsApiLogicService;
        private readonly ExtendedUserContext _extendedUserContext;

        private string _destination;

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand ShowMeCommand { get; }

        public ICommand GetRouteCommand { get; }

        public ICommand CallVolunteerCommand { get; }

        public ICommand CancelRoutingCommand { get; }

        public ICommand BecomeVolunteerCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IFacilitiesLogic facilitiesLogic,
            IGoogleMapsApiLogicService googleMapsApiLogicService,
            ExtendedUserContext extendedUserContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
             _facilitiesLogic = facilitiesLogic;
            _googleMapsApiLogicService = googleMapsApiLogicService;
            _extendedUserContext = extendedUserContext;

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

            GetRouteCommand = BuildPageVmCommand(async () =>
            {
                State = PageStateType.MinorLoading;

                State = PageStateType.Default;
            });

            CallVolunteerCommand = BuildPageVmCommand(async () =>
            {
                State = PageStateType.MinorLoading;

                State = PageStateType.Default;
            });

            CancelRoutingCommand = BuildPageVmCommand(async () =>
            {
                State = PageStateType.MinorLoading;

                MainMap.Polylines.Clear();

                OnPropertyChanged(nameof(IsBuildingRouting));

                State = PageStateType.Default;
            });

            BecomeVolunteerCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await NavigationService.NavigatePopupAsync<VolounteerRegistrationPage>();
                   
                    State = PageStateType.Default;
                },
                () => true);

            MainMap = new CustomMap
            {
                IsIndoorEnabled = true,
                PinClickedCommand = BuildPageVmCommand<Pin>(async pin =>
                {
                    State = PageStateType.Loading;

                    MainMap.Polylines.Clear();

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(async () =>
                        {
                            (bool result, Position myPosition) = await TryGetUserLocation();

                            if (!result)
                            {
                                throw new BusinessLogicException(LogicExceptionType.BadRequest, "Не удалось определить ваше местоположение");
                            }

                            GoogleApi.GoogleDirection googleDirection =
                                await _googleMapsApiLogicService.GetDirections(
                                    new GoogleApiDirectionsRequest
                                    {
                                        TravelMode = "walking",
                                        Origin = myPosition,
                                        Destination = pin.Position,
                                    },
                                    CancellationToken);

                            IEnumerable<Position> positions = Decode(googleDirection.Routes.First().OverviewPolyline.Points);

                            Polyline polyline = new Polyline
                            {
                                StrokeColor = AppColors.Main,
                                StrokeWidth = 4,
                            };

                            foreach (Position position in positions)
                            {
                                polyline.Positions.Add(position);
                            }

                            MainMap.Polylines.Add(polyline);

                            Destination = pin.Address;
                        }));

                    State = PageStateType.Default;

                    OnPropertyChanged(nameof(IsBuildingRouting));
                }),
            };

            MainMap.MapLongClicked += async (sender, e) =>
            {
                await DialogService.DisplayAlert(string.Empty, $"Добавить маркер в {e.Point.Latitude};{e.Point.Longitude}", "Да", "Нет");
            };

            extendedUserContext.UserContextChanged += (sender, e) => OnPropertyChanged(nameof(CanBecomeVolunteer));
        }

        public CustomMap MainMap { get; }

        public bool IsBuildingRouting => MainMap?.Polylines?.Count > 0;

        public bool CanBecomeVolunteer => string.IsNullOrEmpty(_extendedUserContext.FirstName) || string.IsNullOrEmpty(_extendedUserContext.SecondName);

        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        public override async Task OnAppearing()
        {
            if (PageDidAppear)
            {
                return;
            }
            var p = await  _facilitiesLogic.Get(CancellationToken);
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
