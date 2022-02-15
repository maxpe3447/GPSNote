using GpsNote.Extensions;
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

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        public PinListViewModel(INavigationService navigationService,
                                IRepository repository) 
            : base(navigationService)
        {
            Title = Resources.TextControls.Pins;

            _Repository = repository;

            CreatePinCommand = new Command(CreatePinCommandRelease);
            SearchCommand = new Command(SearchCommandRelease);
            DeletePinCommand = new Command(DeletePinCommandRelease);
            EditPinCommand = new Command(EditPinCommandRelease);
            LikeCommand = new Command(LikeCommandRelease);
            EditCommand = new Command(EditCommandRelease);
            DeleteCommand = new Command(DeleteCommandRelease);
        }
        #region -- Properties --
        private ObservableCollection<PinModel> _pinModelsList;
        public ObservableCollection<PinModel> PinModelsList
        {
            get => _pinModelsList;
            set => SetProperty(ref _pinModelsList, value);
        }

        private PinModel _selectedPin;
        public PinModel SelectedPin
        {
            get => _selectedPin;
            set
            {
                SetProperty(ref _selectedPin, value);


                NavigationParameters keyValues = new NavigationParameters();
                keyValues.Add(nameof(SelectedPin), SelectedPin);

                NavigationService.SelectTabAsync(nameof(Views.MapView), keyValues);

                //PinModelsList
            }
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }

        private PinModel _selectedSearchPin;
        public PinModel SelectedSearchPin
        {
            get => _selectedSearchPin;
            set
            {
                SetProperty(ref _selectedSearchPin, value);
                
            }
        }

        #endregion

        #region -- Commands --
        public ICommand CreatePinCommand { get; }
        private async void CreatePinCommandRelease()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(UserId), UserId);
            await NavigationService.NavigateAsync($"NavigationPage/{nameof(Views.CreatePinView)}", parameters);
        }

        public ICommand SearchCommand { get; }
        private void SearchCommandRelease()
        {
            if (string.IsNullOrEmpty(SearchPin))
            {
                return;
            }
            PinModelsList = new ObservableCollection<PinModel>( PinModelsList.Where(x => x.Name.Contains(SearchPin) || x.Description.Contains(SearchPin) || x.Coordinate.Contains(SearchPin)).ToList());
        }

        public ICommand DeletePinCommand { get; }
        private void DeletePinCommandRelease( object selectedpin)
        {
            var pin = selectedpin as PinModel;

            PinModelsList.Remove(pin);

            _Repository.DeleteAsync(pin);
        }

        public ICommand EditPinCommand { get; }
        private async void EditPinCommandRelease(object selectedpin)
        {
            var pin = selectedpin as PinModel;

            string purpose = "ForEdit";

            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(purpose), purpose);
            parameters.Add(nameof(pin), pin);

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(Views.CreatePinView)}", parameters);

        }

        public ICommand LikeCommand { get; }
        private void LikeCommandRelease(object obj)
        {
            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());
            if (model != null)
            {
                int index = PinModelsList.IndexOf(model);
                PinModelsList.Remove(model);
                model.IsFavorit = !model.IsFavorit;
                PinModelsList.Insert(index, model);
                _Repository.UpdateAsync(model);
            }
        }

        public ICommand DeleteCommand { get; }
        private void DeleteCommandRelease(object obj)
        {
            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());
            if (model != null)
            {
                _Repository.DeleteAsync(model);
                int index = PinModelsList.IndexOf(model);
                PinModelsList.Remove(model);
                //model.IsFavorit = !model.IsFavorit;
                //PinModelsList.Insert(index, model);
                //_Repository.UpdateAsync(model);
            }
        }

        public ICommand EditCommand { get; }
        private void EditCommandRelease(object obj)
        {
            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());
            if (model != null)
            {
                //int index = PinModelsList.IndexOf(model);
                //PinModelsList.Remove(model);
                //model.IsFavorit = !model.IsFavorit;
                //PinModelsList.Insert(index, model);
                //_Repository.UpdateAsync(model);
            }
        }
        #endregion

        #region -- Override --

        public override async void Initialize(INavigationParameters parameters)
        {
            //await NavigationService.SelectTabAsync(nameof(Views.MapView));
            if (parameters.ContainsKey(nameof(PinModel.UserId)))
            {
                UserId = parameters.GetValue<int>(nameof(PinModel.UserId));

                PinModelsList = new ObservableCollection<PinModel>(_Repository.GetAllPinsAsync(UserId).Result);

                if (PinModelsList.Count == 0) return;
                for (int i = 0; i < PinModelsList.Count; i++)
                {
                    PinModelsList[i].DeleteCommand = LikeCommand;
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            //if (parameters.ContainsKey(nameof(PinModelsList)))
            //{
            //    parameters.Add(nameof(this.PinModelsList), PinModelsList);
               
            //}

            if (!string.IsNullOrEmpty(SearchPin))
            {
                SearchPin = string.Empty;
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<PinModel>>(nameof(this.PinModelsList), out var initilazingPinNodelList))
            {
                PinModelsList = initilazingPinNodelList;
            }
            int index = -1;
            if(parameters.TryGetValue<PinModel>($"{nameof(PinModel)}_del", out var delPinModel))
            {
                index = PinModelsList.IndexOf(delPinModel);
                PinModelsList.Remove(delPinModel);

            }
            if(parameters.TryGetValue<PinModel>(nameof(PinModel), out var newPinModel))
            {
                if (index == -1)
                {
                    PinModelsList.Add(newPinModel);
                }
                else
                {
                    PinModelsList.Insert(index, newPinModel);
                }
            }


            if (parameters.TryGetValue<int>(nameof(UserId), out var id))
            {
                UserId = id;
            }
            if (PinModelsList.Count == 0) return;
            for (int i = 0; i < PinModelsList.Count; i++)
            {
                PinModelsList[i].DeleteCommand = LikeCommand;
            }
        }

        #endregion

        #region -- Private --
        private int UserId { get; set; }
        IRepository _Repository { get; }
        #endregion
    }
}
