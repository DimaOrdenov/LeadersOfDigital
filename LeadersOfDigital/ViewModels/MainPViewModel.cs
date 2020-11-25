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
using LeadersOfDigital.Views.Map;
using LeadersOfDigital.Definitions.VmLink;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using FFImageLoading.Svg.Forms;
using LeadersOfDigital.Services;

namespace LeadersOfDigital.ViewModels
{
    public class MainPViewModel : PageViewModel
    {
        private readonly IFacilitiesLogic _facilitiesLogic;
        private readonly IGoogleMapsApiLogicService _googleMapsApiLogicService;
        private readonly ExtendedUserContext _extendedUserContext;

        private string _destination;
        private string _searchText;

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand ShowMeCommand { get; }

        public ICommand GetRouteCommand { get; }

        public ICommand CallVolunteerCommand { get; }

        public ICommand CancelRoutingCommand { get; }

        public ICommand BecomeVolunteerCommand { get; }

        public ICommand MicCommand { get; }

        public MainPViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            IFacilitiesLogic facilitiesLogic,
            IGoogleMapsApiLogicService googleMapsApiLogicService,
            ExtendedUserContext extendedUserContext,
            ISpeechToTextService speechToTextService)
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

                MainMap.RemovePins(MainMap.CustomPins.Where(x => x.Type == Definitions.Enums.PinType.Barrier).ToList());

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

            MicCommand = BuildPageVmCommand(
                async () =>
                {
                    if (!(await CrossPermissionsExtension.CheckAndRequestPermissionIfNeeded(
                        new Permissions.BasePermission[]
                        {
                            new Permissions.Speech(),
                            new Permissions.Microphone(),
                        }))
                        .All(x => x.Value == PermissionStatus.Granted))
                    {
                        DialogService.ShowPlatformShortAlert("Нет разрешений");
                        return;
                    }

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(
                            async () =>
                            {
                                speechToTextService.StartSpeechToText();

                                speechToTextService.SpeechRecognitionFinished += (sender, e) =>
                                {
                                    Console.WriteLine(e);

                                    SearchText = e;
                                };
                            }));
                });

            MainMap = new CustomMap
            {
                IsIndoorEnabled = true,
            };

            MainMap.PinClickedEvent += async (sender, e) =>
            {
                const string GET_ROUTE = "Построить маршрут";
                const string GET_INFO = "Получить информацию об объекте";

                switch (await DialogService.DisplayActionSheet(null, "Отмена", null, new[] { GET_ROUTE, GET_INFO }))
                {
                    case GET_ROUTE:
                        State = PageStateType.MinorLoading;

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
                                            Destination = e.Pin.Position,
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

                                    Position barrierPosition1 = positions.ElementAt(positions.Count() / 3);
                                    Position barrierPosition2 = positions.ElementAt(positions.Count() / 4);
                                    Position barrierPosition3 = positions.ElementAt(positions.Count() * 2 / 4);

                                    MainMap.AddPin(new CustomPin
                                    {
                                        Label = "Бордюр",
                                        Address = "address",
                                        Position = barrierPosition1,
                                        Type = Definitions.Enums.PinType.Barrier,
                                    });

                                    MainMap.AddPin(new CustomPin
                                    {
                                        Label = "Ступеньки",
                                        Address = "address",
                                        Position = barrierPosition2,
                                        Type = Definitions.Enums.PinType.Barrier,
                                    });

                                    MainMap.AddPin(new CustomPin
                                    {
                                        Label = "Яма",
                                        Address = "address",
                                        Position = barrierPosition3,
                                        Type = Definitions.Enums.PinType.Barrier,
                                    });

                                    if (i == 0)
                                    {
                                        firstRoutePoint = positions.First();
                                        lastRoutePoint = positions.Last();
                                    }
                                }

                                MainMap.MoveToRegion(MapSpan.FromBounds(Bounds.FromPositions(
                                    new List<Position> { firstRoutePoint, lastRoutePoint })).WithZoom(0.4));

                                Destination = e.Pin.Address;
                            }));

                        State = PageStateType.Default;

                        OnPropertyChanged(nameof(IsBuildingRouting));
                        break;
                    case GET_INFO:
                        State = PageStateType.MinorLoading;

                        await NavigationService.NavigatePopupAsync<FacilityDetailsPage, int>(2);

                        State = PageStateType.Default;
                        break;
                    default:
                        return;
                }
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

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
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

            MainMap.AddPin(await GetNearPin(myPosition));

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

        private async Task<CustomPin> GetNearPin(Position myPosition)
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

            return new CustomPin
            {
                Label = "Магазин",
                Address = address,
                Position = pinPosition,
                Type = Definitions.Enums.PinType.Facility,
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
