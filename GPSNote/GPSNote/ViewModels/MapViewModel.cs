using GPSNote.Models;
using Prism.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using GPSNote.Services.Repository;

namespace GPSNote.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        public MapViewModel(INavigationService navigationService,
                            IRepository repository) 
            : base(navigationService)
        {
            _Repository = repository;

            Title = "Map Page";

            MapClickCommand = new Command(MapClickCommandRelease);
            
            //PinsList = new ObservableCollection<PinModel>
            //{
            //    new PinModel("Addres1", "1", new Position(47.8431096, 35.0874433)),
            //    new PinModel("Addres2", "2", new Position(47.846540, 35.087064)),
            //    new PinModel("Addres3", "3", new Position(47.838393, 35.098817))
            //};
            
        }
        
        #region -- Command -- 
        public ICommand MapClickCommand { get; }
        private void MapClickCommandRelease()
        {
            //SelectedItem = new Pin//"Addres3", "3", new Position(47.838393, 35.098817)
            //{
            //    Address = "Addres3",
            //    Label = "3",
            //    Position = new Position(47.838393, 35.098817)
            //};

            //PinsList.Add(new PinModel("Address", "4", ClickPos));
        }
        #endregion

        #region -- Properties -- 
        private ObservableCollection<PinModel> _pinsList;
        public ObservableCollection<PinModel> PinsList 
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
        }
        private Position _clickPos;
        public Position ClickPos
        {
            get=> _clickPos;
            set
            {
                SetProperty(ref _clickPos, value);
            }
        }

        private Pin _selectedItem;
        public Pin SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        #endregion

        #region -- Override --
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(PinModel.UserId)))
            {
                _UserId = parameters.GetValue<int>(nameof(PinModel.UserId));
            }
            
            //var lst = _Repository.GetAllPinsAsync(_UserId).Result;

            PinsList = new ObservableCollection<PinModel>(_Repository.GetAllPinsAsync(_UserId).Result);
            
        }
        
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            
            parameters.Add(nameof(PinsList), PinsList);
            parameters.Add(nameof(PinModel.UserId), _UserId);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<PinModel>>(nameof(this.PinsList), out var newCounterValue))
            {
                PinsList = newCounterValue;
            }
            if (parameters.TryGetValue<PinModel>(nameof(PinListViewModel.SelectedPin), out var pin))
            {
                SelectedItem = new Pin
                {
                    Address = pin.Name,
                    Position = pin.Position,
                    Label = pin.Description
                };
            }
        }
        #endregion
        #region -- Private --
        private int _UserId { get; set; }
        private IRepository _Repository { get; }
        #endregion
    }
}
