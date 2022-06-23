using GPSNote.Models;
using Prism.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using GPSNote.Helpers;
using Acr.UserDialogs;
using GPSNote.Resources;
using GPSNote.Services.Settings;
using GPSNote.Views;
using GPSNote.Models.Weather;
using GPSNote.Services.Authentication;
using GPSNote.Services.PinManager;
using GPSNote.Extansion;
using GPSNote.Services.LinkManager;
using Prism.Commands;
using GPSNote.Services.Weather;

namespace GPSNote.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {

        readonly private IAuthenticationService _authentication;
        readonly private ISettingsManagerService _settingsManager;
        readonly private IPinManagerService _pinManager;
        readonly private ILinkManagerService _linkManager;
        readonly private IWeatherService _weatherService;

        private const double _maxTabDescriptionHeight = 290;
        private const int _stepTabDescriptionHeight = 30;

        public MapViewModel(
            INavigationService navigationService,
            ISettingsManagerService settingsManager,
            IAuthenticationService authentication,
            IPinManagerService pinManager,
            ILinkManagerService link,
            IWeatherService weatherService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _authentication = authentication;
            _pinManager = pinManager;
            _linkManager = link;
            _weatherService = weatherService;

            TabDescriptionHeight = 0;

            TextResources = new TextResources(typeof(TextControls));
        }

        #region -- Properties -- 
        private List<PinViewModel> _pinViewModelList;
        public List<PinViewModel> PinViewModelList
        {
            get => _pinViewModelList ??= new List<PinViewModel>();
            set => SetProperty(ref _pinViewModelList, value);
        }
        
        private Position _clickPos;
        public Position ClickPos
        {
            get => _clickPos;
            set => SetProperty(ref _clickPos, value);
        }

        private Position _goToPosition;
        public Position GoToPosition
        {
            get => _goToPosition;
            set => SetProperty(ref _goToPosition, value);
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }
       
        private List<PinViewModel> _findedPins;
        public List<PinViewModel> FindedPins
        {
            get => _findedPins;
            set => SetProperty(ref _findedPins, value);
        }

        private PinViewModel _selectedSearchPin;
        public PinViewModel SelectedSearchPin
        {
            get => _selectedSearchPin;
            set => SetProperty(ref _selectedSearchPin, value, () => { GoToPosition = SelectedSearchPin.Position; });
        }

        private bool _isShowingUser;
        public bool IsShowingUser
        {
            get => _isShowingUser;
            set => SetProperty(ref _isShowingUser, value);
        }

        private bool _myLocationButtonEnabled = true;
        public bool MyLocationButtonEnabled
        {
            get => _myLocationButtonEnabled;
            set => SetProperty(ref _myLocationButtonEnabled, value);
        }

        private Pin _pinClick;
        public Pin PinClick
        {
            get => _pinClick;
            set => SetProperty(ref _pinClick, value, () => { InitDescription(); });
        }

        private double _tabDescriptionHeight;
        public double TabDescriptionHeight
        {
            get => _tabDescriptionHeight;
            set => SetProperty(ref _tabDescriptionHeight, value);
        }

        private string _descName;
        public string DescName
        {
            get => _descName ?? string.Empty;
            set => SetProperty(ref _descName, value);
        }

        private string _descCoordinate;
        public string DescCoordinate
        {
            get => _descCoordinate ?? string.Empty;
            set => SetProperty(ref _descCoordinate, value);
        }

        private string _descDescription;
        public string DescDescription
        {
            get => _descDescription ?? string.Empty;
            set => SetProperty(ref _descDescription, value);
        }

        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }

        private CameraUpdate _initialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get => _initialCameraUpdate;
            set => SetProperty(ref _initialCameraUpdate, value);
        }

        private CameraPosition _cameraPosition;
        public CameraPosition CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        private WeatherModel _weatherModel;
        public WeatherModel WeatherModel
        {
            get => _weatherModel;
            set => SetProperty(ref _weatherModel, value);
        }

        private ICommand searchCommand;
        public ICommand SearchCommand { get => searchCommand ??= new DelegateCommand(SearchCommandRelease); }

        private ICommand findMeCommand;
        public ICommand FindMeCommand { get => findMeCommand ??= new DelegateCommand(FindMeCommandRelease); }

        private ICommand exidCommand;
        public ICommand ExidCommand { get => exidCommand ??= new DelegateCommand(ExidCommandRelease); }

        private ICommand pinClickCommand;
        public ICommand PinClickCommand { get => pinClickCommand ??= new DelegateCommand(PinClickCommandRelease); }

        private ICommand mapClickCommand;
        public ICommand MapClickCommand { get => mapClickCommand ??= new DelegateCommand(MapClickCommandRelease); }


        private ICommand goToSettingsCommand;
        public ICommand GoToSettingsCommand { get => goToSettingsCommand ??= new DelegateCommand(GoToSettingsCommandRelease); }


        private ICommand shareCommand;
        public ICommand ShareCommand { get => shareCommand ??= new DelegateCommand(ShareCommandReleaseAsync); }
        
        #endregion

        #region -- Override --
        public async override void Initialize(INavigationParameters parameters)
        {

            MapCameraUpdate(new Position(_settingsManager.LastLatitude,
                                          _settingsManager.LastLongitude));

            PinViewModelList = (await _pinManager.GetAllPins()).DataPinListToViewPinList();

            
            if (_linkManager.IsHave)
            {
                var pos = new Position(_linkManager.GetLinkModel().Latitude, _linkManager.GetLinkModel().Longitude);

                if ((await _pinManager.GetAllPins()).Where(x => x.Latitude == _linkManager.GetLinkModel().Latitude &&
                                                    x.Longitude == _linkManager.GetLinkModel().Longitude)
                                                    .Count() == 0)
                {
                    var res = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                    {
                        Message = UserMsg.HaveALink,
                        OkText = UserMsg.Ok,
                        CancelText = UserMsg.No
                    });
                    if (res)
                    {
                        await _pinManager.AddAsync(new PinViewModel()
                        {
                            Name = _linkManager.GetLinkModel().Name,
                            Description = _linkManager.GetLinkModel().Description,
                            Position = pos
                        }.PinViewToPinData(_authentication.UserId));
                    }
                }

                MapCameraUpdate(pos);

                _linkManager.Clear();
            }

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(nameof(_authentication.UserId), _authentication.UserId);

            _settingsManager.LastLongitude = CameraPosition?.Target.Longitude ?? _settingsManager.LastLongitude;
            _settingsManager.LastLatitude = CameraPosition?.Target.Latitude ?? _settingsManager.LastLatitude;
            _settingsManager.CameraZoom = CameraPosition?.Zoom ?? _settingsManager.CameraZoom;

        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var pins = await _pinManager.GetAllPins();
            PinViewModelList = pins.DataPinListToViewPinList();

            if (parameters.TryGetValue<PinViewModel>(nameof(PinListViewModel.SelectedPin), out var pin))
            {
                GoToPosition = pin.Position;
            }
        }
        #endregion

        #region -- Private --
        private async void ShowTabDescriptionAsync()
        {
            for (int i = 0; i < _maxTabDescriptionHeight; i += _stepTabDescriptionHeight)
            {
                TabDescriptionHeight = i;
                await Task.Delay(1);
            }
        }
        private async void UnShowTabDescriptionAsync()
        {
            int h = (int)TabDescriptionHeight;
            for (int i = h; i > 0; i -= _stepTabDescriptionHeight)
            {
                if (i < 0) i = 0;
                TabDescriptionHeight = i;
                await Task.Delay(1);
            }
            TabDescriptionHeight = 0;
        }

        private void InitDescription()
        {
            PinViewModel model = null;
            try
            {
                model = PinViewModelList.Where(x => x.Position == PinClick.Position).First();
            }
            catch
            {
                UserDialogs.Instance.Alert(UserMsg.DataError);
                return;
            }

            DescName = model?.Name;
            DescCoordinate = model?.Coordinate;
            DescDescription = model?.Description;
        }

        private void MapCameraUpdate(Position position)
        {
            double zoom = Convert.ToDouble(_settingsManager.CameraZoom.ToString());
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(position, zoom));
        }

        private void SearchCommandRelease()
        {
            if (string.IsNullOrEmpty(SearchPin))
            {
                FindedPins = new List<PinViewModel>();
                return;
            }
            FindedPins = PinViewModelList.Where(x => x.Name.Contains(SearchPin) ||
                                                x.Description.Contains(SearchPin) ||
                                                x.Coordinate.Contains(SearchPin))
                                                            .ToList();
        }

        private async void FindMeCommandRelease()
        {
            if ((await Permissions.CheckStatusAsync<Permissions.LocationAlways>()) != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.LocationAlways>();
                return;
            }


            IsShowingUser = true;

            try
            {
                var position = (await Geolocation.GetLastKnownLocationAsync());
                GoToPosition = new Position(position.Latitude, position.Longitude);
            }
            catch
            {
                UserDialogs.Instance.Alert(UserMsg.PlsCheckGPS);
            }
        }

        private void ExidCommandRelease()
        {
            _authentication.UserId = default(int);
            NavigationService.NavigateAsync($"/{nameof(StartPageView)}");
        }

        private void PinClickCommandRelease()
        {
            if (MyLocationButtonEnabled)
            {
                MyLocationButtonEnabled = false;

                ShowTabDescriptionAsync();
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                WeatherModel = _weatherService.GetWeatherInPosition(PinClick.Position);
            }
            else
            {
                UserDialogs.Instance.AlertAsync(UserMsg.WrongInternetConnect);
            }
        }

        private void MapClickCommandRelease()
        {
            MyLocationButtonEnabled = true;
            UnShowTabDescriptionAsync();
        }

        private void GoToSettingsCommandRelease()
            => NavigationService.NavigateAsync(nameof(SettingsView));

        private async void ShareCommandReleaseAsync()
        {
            try
            {

                var uri = new Uri($"{Constants.LINK_PROTOCOL_HTTP}://{Constants.LINK_DOMEN}/{Constants.LINK_GEO}/{PinClick.Position.Latitude}/{PinClick.Position.Longitude}/{PinClick.Label}/{PinClick.Address}");
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = UserMsg.SharePin,
                    Uri = uri.ToString(),
                    Title = UserMsg.SharePin
                });
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        }
        #endregion
    }
}