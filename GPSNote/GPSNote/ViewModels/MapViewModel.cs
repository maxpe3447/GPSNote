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

            Title = Resources.TextControls.Map;

            MapClickCommand = new Command(MapClickCommandRelease);
            SearchCommand = new  Command(SearchCommandRelease);
            ExidCommand = new Command(ExidCommandRelease);
        }
        
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
            set=> SetProperty(ref _clickPos, value);
        }

        private Pin _selectedItem;
        public Pin SelectedItem
        {
            get => _selectedItem;
            set=> SetProperty(ref _selectedItem, value);
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
                SelectedItem = new Pin
                {
                    Label = SelectedSearchPin.Name,
                    Address = SelectedSearchPin.Description,
                    Position = SelectedSearchPin.Position
                };
            }
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
            FindedPins = PinsList.Where(x => x.Name.Contains(SearchPin) || x.Description.Contains(SearchPin) || x.Coordinate.Contains(SearchPin)).ToList();
        }
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

        public ICommand ExidCommand { get; }
        private void ExidCommandRelease()
        {
            NavigationService.NavigateAsync(nameof(Views.StartPageView));
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
