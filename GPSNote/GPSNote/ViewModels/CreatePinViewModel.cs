using Acr.UserDialogs;
using GPSNote.Controls;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Repository;
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
                                  IRepository repository) 
            : base(navigationService)
        {
            _Repository = repository;

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
                    Acr.UserDialogs.UserDialogs.Instance.Alert("Please, select marker!");
                    return;
                }

                //BitmapDescriptor image = new BitmapDescriptor();
                if(PinsList.Count >= 1)
                {
                    PinsList.Clear();
                }

                PinsList.Add(new Pin
                {
                    Label = (string.IsNullOrEmpty(Name)) ? string.Empty : Name,
                    Position = SelectedPosition,
                    Icon = BitmapDescriptorFactory.FromView(new Controls.BindingPinIconView("ic_placeholder.png"))
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
        #endregion

        #region -- Commands --
        public ICommand SaveCommand { get; }
        private async void SaveCommandRelease()
        {
            if(PinsList.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Please, add marker on the map", "Data Error");
                return;
            }

            var pin = new PinModel {
                Name = PinsList.First().Label,
                Description = PinsList.First().Address,
                Position = PinsList.First().Position
            } ;

            pin.Name = Name;
            if(string.IsNullOrEmpty(pin.Name) || pin.Name == _errorMsg)
            {
                await UserDialogs.Instance.AlertAsync("Please, enter \'Name\' of marker", "Data Error");
                return;
            }

            pin.Description = Description;
            if (string.IsNullOrEmpty(pin.Description) || pin.Description == _errorMsg)
            {
                await UserDialogs.Instance.AlertAsync("Please, enter \'Description\' of marker", "Data Error");
                return;
            }

            if (string.IsNullOrEmpty(Longitude) ||
               string.IsNullOrEmpty(Latitude) ||
               !double.TryParse(Longitude, out var longitude) ||
               !double.TryParse(Latitude, out var latitude) ||
               pin.Position != new Position(latitude, longitude))
            {
                await UserDialogs.Instance.AlertAsync("You have bad Longitude, Latitude", "Data Error");

            }

            if (pin.Position == default(Position))
            {
                await UserDialogs.Instance.AlertAsync("Please, enter marker on the map", "Data Error");
                return;
            }

            pin.UserId = _UserId;
            

            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(PinModel), pin);

            if(_oldPin != null)
            {
                pin.Id = _oldPin.Id;
                pin.UserId = _oldPin.UserId;

                await _Repository.UpdateAsync<PinModel>(pin);

                parameters.Add($"{nameof(PinModel)}_del", _oldPin);
            }
            else
            {
                await _Repository.InsertAsync<PinModel>(pin);
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
            //MyLocationButtonEnabled = false;

            try
            {
                GoToPosition = new Position(Geolocation.GetLastKnownLocationAsync().Result.Latitude, Geolocation.GetLastKnownLocationAsync().Result.Longitude);
            }
            catch
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert("Please, check your GPS settings.");
            }
        }
        #endregion

        #region -- Overrides --
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.TryGetValue<int>(nameof(PinModel.UserId), out int id)){
                _UserId = id;
            }

            string key = "purpose";
            if (parameters.ContainsKey(key))
            {
                Title = parameters.GetValue<string>(key);

                PinModel pin = parameters.GetValue<PinModel>(nameof(pin));
                _oldPin = pin;
                Name = pin.Name;
                Description = pin.Description;
                Longitude = pin.Coordinate;


                PinsList.Add(new Pin
                {
                    Label = Name,
                    Address = Description,
                    Position = pin.Position
                });

                GoToPosition = pin.Position;
            }
        }
        #endregion

        #region -- Private --
        private PinModel _oldPin = null;
        private string _errorMsg { get; } = "NaN";
        private IRepository _Repository { get; }
        private int _UserId { get; set; }
        #endregion
    }
}