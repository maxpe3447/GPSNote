using GPSNote.Models;
using Prism.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using GPSNote.Services.Repository;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using GPSNote.Helpers;
using Acr.UserDialogs;
using GPSNote.Resources;
using System.Collections.Specialized;
using GPSNote.Services.Settings;
using GPSNote.Views;
using GPSNote.Models.Weather;

namespace GPSNote.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        public MapViewModel(INavigationService navigationService,
                            IRepository repository,
                            ISettingsManager settingsManager) 
            : base(navigationService)
        {
            _Repository = repository;
            _SettingsManager = settingsManager;

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
        private ObservableCollection<Pin> _pinsList;
        public ObservableCollection<Pin> PinsList 
        {
            get => _pinsList ?? new ObservableCollection<Pin>();
            set => SetProperty(ref _pinsList, value);
        }
        private ObservableCollection<PinModel> _pinModelsList;
        public ObservableCollection<PinModel> PinModelsList
        {
            get => _pinModelsList ?? new ObservableCollection<PinModel>();
            set => SetProperty(ref _pinModelsList, value);
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
       
        private List<PinModel> _findedPins;
        public List<PinModel> FindedPins
        {
            get => _findedPins;
            set => SetProperty(ref _findedPins, value);
        }

        private PinModel _selectedSearchPin;
        public PinModel SelectedSearchPin
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
                FindedPins = new List<PinModel>();
                return;
            }
            FindedPins = PinModelsList.Where(x => x.Name.Contains(SearchPin) || 
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

            if (parameters.ContainsKey(nameof(PinModel.UserId)))
            {
                _UserId = parameters.GetValue<int>(nameof(PinModel.UserId));
            }

            PinModelsList = new ObservableCollection<PinModel>(
                _Repository.GetAllPinsAsync(_UserId).Result);

            PinModelsList.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var item = PinModelsList.Last();
                    PinsList.Add(new Pin
                    {
                        Label = item.Name,
                        Position = item.Position,
                        Icon = BitmapDescriptorFactory.FromView(
                            new Controls.BindingPinIconView((ImageSource)App.Current
                                                                            .Resources[ImageNames.ic_placeholder]))
                    });
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.OldItems[0] is PinModel removeObj) {
                        var pin = PinsList.FirstOrDefault(x => x.Position == removeObj.Position);
                        PinsList.Remove(pin);
                    }
                }
            };
            _ = InitPinsListAsync(); 
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (!initilize)
            {
                parameters.Add(nameof(PinModelsList), PinModelsList);
            }
            parameters.Add(nameof(PinModel.UserId), _UserId);

            _SettingsManager.LastLongitude = CameraPosition?.Target.Longitude ?? _SettingsManager.LastLongitude;
            _SettingsManager.LastLatitude = CameraPosition?.Target.Latitude ?? _SettingsManager.LastLatitude;
            _SettingsManager.CameraZoom = CameraPosition?.Zoom ?? _SettingsManager.CameraZoom;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue<LinkModel>(nameof(LinkModel), out var link))
            {
                var res = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig 
                { 
                    Message =UserMsg.HaveALink, 
                    OkText = UserMsg.Ok,
                    CancelText = UserMsg.No});
                if (res)
                {
                    var pos = new Position(link.Latitude, link.Longitude);
                    PinModelsList.Add(new PinModel 
                    { 
                        Position = pos,
                        Name = link.Name,
                        Description = link.Description
                    });
                    GoToPosition = pos;
                }
            }
            if (parameters.TryGetValue<PinModel>(nameof(PinListViewModel.SelectedPin), out var pin))
            {
                GoToPosition = pin.Position;
            }
        }
        #endregion

        #region -- Private --
        private bool initilize = false;
        private int _UserId { get; set; }
        private IRepository _Repository { get; }
        private ISettingsManager _SettingsManager { get; }
        private const double _maxTabDescriptionHeight = 290;
        private const int _stepTabDescriptionHeight = 30;
        
        
        private async void ShowTabDescriptionAsync()
        {
           await Task.Run(async ()=>
           {
               for (int i = 0; i < _maxTabDescriptionHeight; i += _stepTabDescriptionHeight)
               {
                   TabDescriptionHeight = i;
                   await Task.Delay(1);
               }
           });

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
           PinModel model = null;
            try
            {
                model = PinModelsList.Where(x => x.Position == PinClick.Position).First();
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

        private async Task InitPinsListAsync()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < PinModelsList.Count; i++)
                {
                    PinsList.Add(new Pin
                    {
                        Label = PinModelsList[i].Name,
                        Position = PinModelsList[i].Position,
                        Icon = BitmapDescriptorFactory.FromView(
                            new Controls.BindingPinIconView((ImageSource)App.Current
                                                                            .Resources[ImageNames.ic_placeholder]))
                    });
                }
            });

        }

        private void InitCameraUpdate()
        {
            double zoom = Convert.ToDouble(_SettingsManager.CameraZoom.ToString());
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(new Position(_SettingsManager.LastLatitude,
                                                _SettingsManager.LastLongitude),zoom));
        }
        #endregion
    }
}