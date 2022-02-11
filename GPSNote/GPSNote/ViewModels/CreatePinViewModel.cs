using GPSNote.Models;
using GPSNote.Services.Repository;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
//using Xamarin.Forms.Maps;

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

        private ObservableCollection<PinModel> _pinsList;
        public ObservableCollection<PinModel> PinsList
        {
            get => _pinsList ?? (_pinsList = new ObservableCollection<PinModel>());
            set => SetProperty(ref _pinsList, value);
        }

        //private Position _selectedPosition;
        //public Position SelectedPosition
        //{
        //    get => _selectedPosition;
        //    set
        //    {
        //        SetProperty(ref _selectedPosition, value);
        //        SelectedItem = new Pin
        //        {
        //            Label = (string.IsNullOrEmpty(Name)) ? "Nan" : Name,
        //            Address = (string.IsNullOrEmpty(Description)) ? "Nan" : Description,
        //            Position = SelectedPosition
        //        };

        //        Position = $"{SelectedPosition.Latitude.ToString("0.000000")} {SelectedPosition.Longitude.ToString("0.000000")}";

        //        if (string.IsNullOrEmpty(Position))
        //        {
        //            Acr.UserDialogs.UserDialogs.Instance.Alert("Please, select marker!");
        //            return;
        //        }
        //        if (PinsList.Count > 0)
        //        {
        //            PinsList.Clear();
        //        }
        //        PinsList.Add(new PinModel(
        //            (string.IsNullOrEmpty(Name)) ? _errorMsg : Name,
        //            (string.IsNullOrEmpty(Description)) ? _errorMsg : Description,
        //            SelectedPosition));
        //    }
        //}
        //private Pin _selectedItem;
        //public Pin SelectedItem
        //{
        //    get => _selectedItem;
        //    set
        //    {
        //        SetProperty(ref _selectedItem, value);

        //    }
        //}

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
            var pin = PinsList.First();

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

            //if(pin.Position == default(Position))
            //{
            //    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please, enter marker on the map", "Data Error");
            //    return;
            //}

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


                //PinsList.Add(new PinModel( Name, Description, pin.Position));

                //SelectedItem = new Pin
                //{
                //    Label = Name,
                //    Address = Description,
                //    Position = pin.Position
                //};
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