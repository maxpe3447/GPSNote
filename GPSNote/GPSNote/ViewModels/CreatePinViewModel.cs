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
using GPSNote.Extansion;
using GPSNote.Services.Authentication;
using Prism.Commands;

namespace GPSNote.ViewModels
{
    public class CreatePinViewModel : ViewModelBase
    {

        private PinViewModel _oldPin = null;
        private bool _isEditPin;

        readonly private IAuthenticationService _authentication;
        readonly private IPinManagerService _pinManager;
        readonly private ISettingsManagerService _settingsManager;

        public CreatePinViewModel(INavigationService navigationService,
                                  IPinManagerService pinManager,
                                  ISettingsManagerService settingsManager,
                                  IAuthenticationService authentication) 
            : base(navigationService)
        {
            _pinManager = pinManager;
            _settingsManager = settingsManager;
            _authentication = authentication;

            TextResources = new TextResources(typeof(Resources.TextControls));
        }
        #region -- Propirties -- 

        private List<PinViewModel> _pinViewList;
        public List<PinViewModel> PinsList
        {
            get => _pinViewList ?? (_pinViewList = new List<PinViewModel>());
            set => SetProperty(ref _pinViewList, value);
        }

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set => SetProperty(ref _selectedPosition, value, OnChangePosition);
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

        private ICommand saveCommand;
        public ICommand SaveCommand { get => saveCommand ??= new DelegateCommand(SaveCommandRelease); }

        private ICommand cancelCommand;
        public ICommand CancelCommand { get => cancelCommand ??= new DelegateCommand(CancelCommandRelease); }


        private ICommand findMeCommand;
        public ICommand FindMeCommand { get => findMeCommand ??= new DelegateCommand(FindMeCommandRelease); }
        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            double zoom = Convert.ToDouble(_settingsManager.CameraZoom.ToString());
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(new Position(_settingsManager.LastLatitude,
                                                _settingsManager.LastLongitude),
                                                zoom));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Title = Resources.TextControls.AddPin;
            

            if (parameters.TryGetValue(nameof(_isEditPin), out _isEditPin) && _isEditPin)
            {

                _oldPin = parameters.GetValue<PinViewModel>(nameof(PinViewModel));
                
                Name = _oldPin.Name;
                Description = _oldPin.Description;
                Longitude = _oldPin.Position.Longitude.ToString("0.000000");
                Latitude = _oldPin.Position.Latitude.ToString("0.000000");

                PinsList = new List<PinViewModel>
                { 
                    new PinViewModel
                    {
                        Name = Name,
                        Address = Description,
                        Position = _oldPin.Position
                    }
                };

                GoToPosition = _oldPin.Position;
                Title = Resources.TextControls.EditPin;
            }
        }
        #endregion

        #region -- Private --
        private async void SaveCommandRelease()
        {
            if (PinsList.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsAddPin,
                                                      Resources.UserMsg.DataError);
                return;
            }
            var pin = new PinViewModel
            {
                Name = PinsList.First().Name,
                Description = PinsList.First().Address,
                Position = PinsList.First().Position
            };
            pin.Name = Name;
            if (string.IsNullOrEmpty(pin.Name))
            {
                await UserDialogs.Instance.AlertAsync(Resources.UserMsg.PlsEnterName,
                                                      Resources.UserMsg.DataError);
                return;
            }
            pin.Description = Description ?? string.Empty;

            if (string.IsNullOrEmpty(Longitude) ||
               string.IsNullOrEmpty(Latitude) ||
               !double.TryParse(Longitude, out var longitude) ||
               !double.TryParse(Latitude, out var latitude) ||
               !pin.Position.Latitude.ToString().Contains(Latitude.Remove(Latitude.Length - 1, 1)) ||
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Geocoder geoCoder = new Geocoder();

                var address = (await geoCoder.GetAddressesForPositionAsync(pin.Position))
                                             .FirstOrDefault()
                                             .Split(new[] { ' ' },
                                             StringSplitOptions.RemoveEmptyEntries)
                                             .ToList();

                address.Remove(address.First());
                address.Remove(address.First());

                pin.Address = string.Join(" ", address);
            }
            if (_oldPin != null)
            {
                await _pinManager.UpdateAsync(pin.PinViewToPinData(await _pinManager.GetAllPins()));
            }
            else
            {
                pin.IsVisable = true;
                await _pinManager.AddAsync(pin.PinViewToPinData(_authentication.UserId));
            }

            await NavigationService.GoBackAsync();
        }

        private async void CancelCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        private async void FindMeCommandRelease()
        {
            IsShowingUser = true;

            try
            {
                var position = await Geolocation.GetLastKnownLocationAsync();

                GoToPosition = new Position(position.Latitude, position.Longitude);
            }
            catch
            {
                UserDialogs.Instance.Alert(Resources.UserMsg.PlsCheckGPS);
            }
        }

        private void OnChangePosition()
        {
            Longitude = SelectedPosition.Longitude.ToString("0.000000");
            Latitude = SelectedPosition.Latitude.ToString("0.000000");

            if (string.IsNullOrEmpty(Longitude))
            {
                UserDialogs.Instance.Alert(Resources.UserMsg.PlsSelectPin);
                return;
            }

            PinsList = new List<PinViewModel>
                {
                    new PinViewModel
                    {
                        Name = (string.IsNullOrEmpty(Name)) ? string.Empty : Name,
                        Position = SelectedPosition
                    }
                };

            GoToPosition = SelectedPosition;
        }
        #endregion
    }
}