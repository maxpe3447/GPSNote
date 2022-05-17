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

namespace GPSNote.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        readonly private IAuthentication _authentication;
        readonly private ISettingsManager _settingsManager;
        readonly private IPinManager _pinManager;
        readonly private ILinkManager _linkManager;
        public MapViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IAuthentication authentication,
            IPinManager pinManager,
            ILinkManager link)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _authentication = authentication;
            _pinManager = pinManager;
            _linkManager = link;

            TabDescriptionHeight = 0;

            FindMeCommand = new Command(FindMeCommandRelease);
            SearchCommand = new  Command(SearchCommandRelease);
            ExidCommand = new Command(ExidCommandRelease);
            PinClickCommand = new Command(PinClickCommandRelease);
            MapClickCommand = new Command(MapClickCommandRelease);
            ShareCommand = new Command(ShareCommandReleaseAsync);
            GoToSettingsCommand = new Command(GoToSettingsCommandRelease);

            TextResources = new TextResources(typeof(TextControls));
        }

        #region -- Properties -- 
        private List<PinViewModel> _pinViewModelList;
        public List<PinViewModel> PinViewModelList
        {
            get => _pinViewModelList ?? new List<PinViewModel>();
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
            set
            {
                SetProperty(ref _selectedSearchPin, value);
                GoToPosition = SelectedSearchPin.Position;
            }
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
            set
            {
                SetProperty(ref _pinClick, value);
                InitDescription();
            }
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
        #endregion

        #region -- Command -- 
        public ICommand SearchCommand { get; }
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
        public ICommand FindMeCommand { get; }
        private void FindMeCommandRelease()
        {
            IsShowingUser = true;

            try
            {
                GoToPosition = new Position(Geolocation.GetLastKnownLocationAsync().Result.Latitude, 
                                            Geolocation.GetLastKnownLocationAsync().Result.Longitude);
            }
            catch
            {
                UserDialogs.Instance.Alert(UserMsg.PlsCheckGPS);
            }
        }

        public ICommand ExidCommand { get; }
        private void ExidCommandRelease()
        {
            _authentication.UserId = default(int);
            NavigationService.NavigateAsync($"/{nameof(StartPageView)}");
        }

        public ICommand PinClickCommand { get; }
        private void PinClickCommandRelease()
        {
            if (MyLocationButtonEnabled)
            {
                MyLocationButtonEnabled = false;
                
                ShowTabDescriptionAsync();
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                WeatherModel = Weather.GetResponse(PinClick.Position.Latitude, PinClick.Position.Longitude);
            }
            else
            {
                UserDialogs.Instance.AlertAsync(UserMsg.WrongInternetConnect);
            }
        }
        public ICommand MapClickCommand { get; }
        private void MapClickCommandRelease()
        {
            MyLocationButtonEnabled = true;
            UnShowTabDescriptionAsync();
            
        }

        public ICommand GoToSettingsCommand { get; }
        private void GoToSettingsCommandRelease()
        {
            NavigationService.NavigateAsync(nameof(SettingsView));
        }

        public ICommand ShareCommand { get; }
        private async void ShareCommandReleaseAsync()
        {
            try
            {
                var uri = new Uri($"http://GPSNote.App/geo/{PinClick.Position.Latitude}/{PinClick.Position.Longitude}/{PinClick.Label}/{PinClick.Address}");
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = UserMsg.SharePin,
                    Uri = uri.ToString(),
                    Title = UserMsg.SharePin
                });
            }catch(Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        }
        #endregion

        #region -- Override --
        public override void Initialize(INavigationParameters parameters)
        {
            InitCameraUpdate();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            parameters.Add(nameof(_authentication.UserId), _authentication.UserId);

            _settingsManager.LastLongitude = CameraPosition?.Target.Longitude ?? _settingsManager.LastLongitude;
            _settingsManager.LastLatitude = CameraPosition?.Target.Latitude ?? _settingsManager.LastLatitude;
            _settingsManager.CameraZoom = CameraPosition?.Zoom ?? _settingsManager.CameraZoom;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            PinViewModelList = _pinManager.GetAllPins(_authentication.UserId)
                                          .DataPinListToViewPinList();
//TODO: LINK
            if (_linkManager.IsHave)
            {
                var res = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = UserMsg.HaveALink,
                    OkText = UserMsg.Ok,
                    CancelText = UserMsg.No
                });
                if (res)
                {
                    var pos = new Position(_linkManager.GetLinkModel().Latitude, _linkManager.GetLinkModel().Longitude);
                    await _pinManager.InsertAsync( new PinViewModel()
                    {
                        Name = _linkManager.GetLinkModel().Name,
                        Description = _linkManager.GetLinkModel().Description,
                        Position = pos
                    }.PinViewToPinData(_authentication.UserId));
                    
                    GoToPosition = pos;
                }
            }
            if (parameters.TryGetValue<PinViewModel>(nameof(PinListViewModel.SelectedPin), out var pin))
            {
                GoToPosition = pin.Position;
            }
        }
        #endregion

        #region -- Private --
        private const double _maxTabDescriptionHeight = 290;
        private const int _stepTabDescriptionHeight = 30;


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
                if(i < 0) i = 0;
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

        private void InitCameraUpdate()
        {
            double zoom = Convert.ToDouble(_settingsManager.CameraZoom.ToString());
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(new Position(_settingsManager.LastLatitude,
                                                _settingsManager.LastLongitude),zoom));
        }
        #endregion
    }
}