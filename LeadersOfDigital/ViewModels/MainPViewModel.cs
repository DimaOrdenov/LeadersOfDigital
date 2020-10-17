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
using LeadersOfDigital.Views.Map;
using LeadersOfDigital.Definitions.VmLink;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
                    MoveToPosition(myPosition, Distance.FromKilometers(1));
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

                            Position firstRoutePoint = new Position();
                            Position lastRoutePoint = new Position();

                            for (double i = 0; i < 3; i++)
                            {
                                Position waypoint = new Position(myPosition.Latitude + (i % 2 == 0 ? 0.005 : 0), myPosition.Longitude + (i > 0 ? 0.005 : 0));

                                GoogleApi.GoogleDirection googleDirection = await _googleMapsApiLogicService.GetDirections(
                                    new GoogleApiDirectionsRequest
                                    {
                                        Origin = myPosition,
                                        Destination = pin.Position,
                                        Waypoint = waypoint,
                                    },
                                    CancellationToken);

                                IEnumerable<Position> positions = Decode(googleDirection.Routes.First().OverviewPolyline.Points);

                                Polyline polyline = new Polyline
                                {
                                    StrokeColor = i == 0 ? AppColors.Main : AppColors.LightMain,
                                    StrokeWidth = 4,
                                    IsClickable = true,
                                    ZIndex = i == 0 ? 100 : 0,
                                };

                                polyline.Clicked += (sender, e) =>
                                {
                                    MainMap.Polylines.ForEach(x =>
                                    {
                                        x.StrokeColor = AppColors.LightMain;
                                        x.ZIndex = 0;
                                    });

                                    polyline.StrokeColor = AppColors.Main;
                                    polyline.ZIndex = 100;
                                };

                                foreach (Position position in positions)
                                {
                                    polyline.Positions.Add(position);
                                }

                                MainMap.Polylines.Add(polyline);

                                if (i == 0)
                                {
                                    firstRoutePoint = positions.First();
                                    lastRoutePoint = positions.Last();
                                }
                            }

                            MainMap.MoveToRegion(MapSpan.FromBounds(Bounds.FromPositions(
                                new List<Position> { firstRoutePoint, lastRoutePoint })).WithZoom(0.4));

                            Destination = pin.Address;
                        }));

                    State = PageStateType.Default;

                    OnPropertyChanged(nameof(IsBuildingRouting));
                }),
            };

            MainMap.MapLongClicked += async (sender, e) =>
                    {
                        MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(e.Point.Latitude, e.Point.Longitude), MainMap.VisibleRegion.Radius));

                        if (await DialogService.DisplayAlert(
                            "Дорожное событие",
                            "Вы можете создать дорожное событие, указав общую проблематику, описать пробему и приложить фотографии",
                            "Добавить",
                            "Отмена"))
                        {
                            State = PageStateType.MinorLoading;

                            string address = string.Empty;

                            try
                            {
                                Geocoder geocoder = new Geocoder();
                                IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(e.Point);

                                address = addresses.FirstOrDefault()?.Split("\n")?.FirstOrDefault() ?? "ул.Пушкина, д.1";
                            }
                            catch (Exception ex)
                            {
                                DebuggerService.Log(ex);
                            }

                            await NavigationService.NavigatePopupAsync<AddMarkerPage, AddMarkerVmLink>(new AddMarkerVmLink(e.Point.Latitude, e.Point.Longitude, address));

                            State = PageStateType.Default;
                        }
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

            State = PageStateType.MinorLoading;

            await MainMap.MoveCamera(CameraUpdateFactory.NewPositionZoom(new Position(55.751314, 37.627335), 0.5));

            (bool result, Position myPosition) = await TryGetUserLocation();

            if (result)
            {
                MoveToPosition(myPosition, Distance.FromKilometers(6));
            }

            MainMap.Pins.Add(await GetNearPin(myPosition));

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

        private void MoveToPosition(Position position, Distance distance) =>
            MainMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(position, distance));

        private async Task<Pin> GetNearPin(Position myPosition)
        {
            Position pinPosition = new Position(myPosition.Latitude + 0.01, myPosition.Longitude + 0.01);
            string address = string.Empty;

            try
            {
                Geocoder geocoder = new Geocoder();
                IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(pinPosition);

                address = addresses.FirstOrDefault()?.Split("\n")?.FirstOrDefault() ?? "ул.Пушкина, д.1";
            }
            catch (Exception ex)
            {
                DebuggerService.Log(ex);
            }

            return new Pin
            {
                Label = "Магазин",
                Address = address,
                Position = pinPosition,
            };
        }

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
