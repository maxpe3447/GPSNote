using GPSNote.Models;
using Prism.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
//using Xamarin.Forms.Maps;
using GPSNote.Services.Repository;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;

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

            FindMeCommand = new Command(FindMeCommandRelease);
            SearchCommand = new  Command(SearchCommandRelease);
            ExidCommand = new Command(ExidCommandRelease);
            
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
            set
            {
                SetProperty(ref _clickPos, value);
            }
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

        private bool _myLocationButtonEnabled;
        public bool MyLocationButtonEnabled
        {
            get => _myLocationButtonEnabled;
            set => SetProperty(ref _myLocationButtonEnabled, value);
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
            FindedPins = PinModelsList.Where(x => x.Name.Contains(SearchPin) || x.Description.Contains(SearchPin) || x.Coordinate.Contains(SearchPin)).ToList();
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
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert("Please, check your GPS settings.");
            }
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

            PinModelsList = new ObservableCollection<PinModel>(_Repository.GetAllPinsAsync(_UserId).Result);
            PinModelsList.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var item = PinModelsList.Last();
                    PinsList.Add(new Pin
                    {
                        Label = item.Name,
                        Position = item.Position,
                        Icon = BitmapDescriptorFactory.FromView(new Controls.BindingPinIconView("ic_placeholder.png"))
                    });
                }
            };
            InitPinsListAsync();
            
        }

        private async Task InitPinsListAsync()
        {
            await Task.Run(() =>
            {
                for(int i = 0; i < PinModelsList.Count; i++)
                {
                    PinsList.Add(new Pin
                    {
                        Label = PinModelsList[i].Name,
                        Position = PinModelsList[i].Position,
                        Icon = BitmapDescriptorFactory.FromView(new Controls.BindingPinIconView("ic_placeholder.png"))
                    });
                }
            });

        } 

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (!initilize)
            {
                parameters.Add(nameof(PinModelsList), PinModelsList);
            }
            parameters.Add(nameof(PinModel.UserId), _UserId);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //if (parameters.TryGetValue<List<PinModel>>(nameof(PinModelsList), out var newCounterValue))
            //{
            //    PinModelsList = newCounterValue;
            //}
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
        #endregion
    }
}