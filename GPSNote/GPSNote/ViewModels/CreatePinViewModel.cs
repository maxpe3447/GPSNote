using GPSNote.Controls;
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

            Title = "Create Pin";

            SaveCommand = new Command(SaveCommandRelease);
            CancelCommand = new Command(CancelCommandRelease);
        }
        #region -- Propirties -- 

        private ObservableCollection<Pin> _pinModelssList;
        public ObservableCollection<Pin> PinsList
        {
            get => _pinModelssList ?? (_pinModelssList = new ObservableCollection<Pin>());
            set => SetProperty(ref _pinModelssList, value);
        }

        //private List<Pin> _pinsList;
        //public List<Pin> PinsList
        //{
        //    get => _pinsList ?? (_pinsList = new List<Pin>());
        //    set => SetProperty(ref _pinsList, value);
        //}

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                SetProperty(ref _selectedPosition, value);
                
                Position = $"{SelectedPosition.Latitude.ToString("0.000000")} {SelectedPosition.Longitude.ToString("0.000000")}";

                if (string.IsNullOrEmpty(Position))
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
                    //Address = Description,
                    Position = SelectedPosition,
                    Icon = BitmapDescriptorFactory.FromView(new Controls.BindingPinIconView("ic_pin.png"))
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

        private string _position;
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
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
        #endregion

        #region -- Commands --
        public ICommand SaveCommand { get; }
        private async void SaveCommandRelease()
        {
            if(PinsList.Count == 0)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, add marker on the map", "Data Error");
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
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, enter \'Name\' of marker", "Data Error");
                return;
            }

            pin.Description = Description;
            if (string.IsNullOrEmpty(pin.Description) || pin.Description == _errorMsg)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, enter \'Description\' of marker", "Data Error");
                return;
            }

            if (pin.Position == default(Position))
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, enter marker on the map", "Data Error");
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
                Position = pin.Coordinate;


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