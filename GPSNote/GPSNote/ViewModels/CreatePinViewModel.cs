using Acr.UserDialogs;
using GPSNote.Controls;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.PinManager;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using GPSNote.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration;

namespace GPSNote.ViewModels
{
    public class CreatePinViewModel : ViewModelBase
    {
        public CreatePinViewModel(INavigationService navigationService,
                                  IPinManager pinManager,
                                  ISettingsManager settingsManager) 
            : base(navigationService)
        {
            _PinManager = pinManager;
            _SettingsManager = settingsManager;

            TextResources = new TextResources(typeof(Resources.TextControls));

            SaveCommand = new Command(SaveCommandRelease);
            CancelCommand = new Command(CancelCommandRelease);
            FindMeCommand = new Command(FindMeCommandRelease);
        }
        #region -- Propirties -- 

        private ObservableCollection<Pin> _pinModelssList;
        public ObservableCollection<Pin> PinsList
        {
            get => _pinModelssList ?? (_pinModelssList = new ObservableCollection<Pin>());
            set => SetProperty(ref _pinModelssList, value);
        }

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                SetProperty(ref _selectedPosition, value);
                
                Longitude = SelectedPosition.Longitude.ToString("0.000000");
                Latitude = SelectedPosition.Latitude.ToString("0.000000");

                if (string.IsNullOrEmpty(Longitude))
                {
                    UserDialogs.Instance.Alert(Resources.UserMsg.PlsSelectPin);
                    return;
                }

                if(PinsList.Count >= 1)
                {
                    PinsList.Clear();
                }

                PinsList.Add(new Pin
                {
                    Label = (string.IsNullOrEmpty(Name)) ? string.Empty : Name,
                    Position = SelectedPosition,
                    Icon = BitmapDescriptorFactory.FromView(
                        new Controls.BindingPinIconView((ImageSource)App.Current
                                                                        .Resources[Resources
                                                                        .ImageNames
                                                                        .ic_placeholder]))
                });
                GoToPosition = SelectedPosition;
            }
        }
        private Position _goToPosition;
        public Position GoToPosition
        {
            get => _goToPosition;
            set => SetProperty(ref _goToPosition, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private bool _isShowingUser;
        public bool IsShowingUser
        {
            get => _isShowingUser;
            set => SetProperty(ref _isShowingUser, value);
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
        #endregion

        #region -- Commands --
        public ICommand SaveCommand { get; }
        private async void SaveCommandRelease()
        {
            if(PinsList.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsAddPin, 
                                                      Resources.UserMsg.DataError);
                return;
            }

            var pin = new PinModel {
                Name = PinsList.First().Label,
                Description = PinsList.First().Address,
                Position = PinsList.First().Position
            } ;

            pin.Name = Name;
            if(string.IsNullOrEmpty(pin.Name))
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsEnterName,
                                                      Resources.UserMsg.DataError);
                return;
            }

            pin.Description = Description;
            if (string.IsNullOrEmpty(pin.Description))
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsEnterDescroption, 
                                                      Resources.UserMsg.DataError);
                return;
            }

            if (string.IsNullOrEmpty(Longitude) ||
               string.IsNullOrEmpty(Latitude) ||
               !double.TryParse(Longitude, out var longitude) ||
               !double.TryParse(Latitude, out var latitude) ||
               !pin.Position.Latitude.ToString().Contains(Latitude.Remove(Latitude.Length-1, 1))||
               !pin.Position.Longitude.ToString().Contains(Longitude.Remove(Longitude.Length - 1, 1)))
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.BadLongLat,
                                                      Resources.UserMsg.DataError);
                return;
            }

            if (pin.Position == default(Position))
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsAddPin, 
                                                      Resources.UserMsg.DataError);
                return;
            }

            pin.UserId = _UserId;
            
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Geocoder geoCoder = new Geocoder();

                var address = (await geoCoder.GetAddressesForPositionAsync(pin.Position))
                                             .FirstOrDefault()
                                             .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                             .ToList();

                address.Remove(address.First());
                address.Remove(address.First());

                pin.Address = string.Join(" ", address);
            }
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(PinModel), pin);

            if(_oldPin != null)
            {
                pin.Id = _oldPin.Id;
                pin.UserId = _oldPin.UserId;

                await _PinManager.UpdateAsync(pin);

                var pinForDel = _oldPin;
                parameters.Add(nameof(pinForDel), pinForDel);
                
            }
            else
            {
                await _PinManager.InsertAsync(pin);
            }

            await NavigationService.GoBackAsync(parameters);
        }
        public ICommand CancelCommand { get; }
        private async void CancelCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        public ICommand FindMeCommand { get; }
        private void FindMeCommandRelease()
        {
            IsShowingUser = true;

            try
            {
                GoToPosition = new Position(
                    Geolocation.GetLastKnownLocationAsync().Result.Latitude, 
                    Geolocation.GetLastKnownLocationAsync().Result.Longitude);
            }
            catch
            {
                UserDialogs.Instance.Alert(Resources.UserMsg.PlsCheckGPS);
            }
        }
        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(new Position(_SettingsManager.LastLatitude,
                                                _SettingsManager.LastLongitude),
                                                _SettingsManager.CameraZoom));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.TryGetValue<int>(nameof(PinModel.UserId), out int id)){
                _UserId = id;
                Title = Resources.TextControls.AddPin;
            }

            if (parameters.TryGetValue(nameof(_isEditPin), out _isEditPin) && _isEditPin)
            {

                _oldPin = parameters.GetValue<PinModel>(nameof(PinModel));
                
                Name = _oldPin.Name;
                Description = _oldPin.Description;
                Longitude = _oldPin.Coordinate;


                PinsList.Add(new Pin
                {
                    Label = Name,
                    Address = Description,
                    Position = _oldPin.Position
                });

                GoToPosition = _oldPin.Position;
                Title = Resources.TextControls.EditPin;
            }
        }
        #endregion

        #region -- Private --
        private PinModel _oldPin = null;
        private bool _isEditPin;
        private IPinManager _PinManager { get; }
        private ISettingsManager _SettingsManager { get; }
        private int _UserId { get; set; }
        #endregion
    }
}