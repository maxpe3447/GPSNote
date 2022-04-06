using GpsNote.Extensions;
using GPSNote.Extansion;
using GPSNote.Models;
using GPSNote.Resources;
using GPSNote.Services.Authentication;
using GPSNote.Services.PinManager;
using GPSNote.Services.Repository;
using GPSNote.Views;
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
        readonly private IAuthentication _authentication;
        readonly private IPinManager _pinManager;

        public PinListViewModel(INavigationService navigationService,
                                IPinManager pinManager,
                                IAuthentication authentication) 
            : base(navigationService)
        {
            Title = TextControls.Pins;

            _pinManager = pinManager;
            _authentication = authentication;

            CreatePinCommand = new Command(CreatePinCommandRelease);
            SearchCommand = new Command(SearchCommandRelease);
            LikeCommand = new Command(LikeCommandRelease);
            EditCommand = new Command(EditCommandRelease);
            DeleteCommand = new Command(DeleteCommandRelease);
            ExidCommand = new Command(ExidCommandRelease);
            GoToSettingsCommand = new Command(GoToSettingsCommandRelease);
        }
        #region -- Properties --
        private List<PinViewModel> _pinViewList;
        public List<PinViewModel> PinViewList
        {
            get => _pinViewList;
            set => SetProperty(ref _pinViewList, value);
        }

        private PinViewModel _selectedPin;
        public PinViewModel SelectedPin
        {
            get => _selectedPin;
            set
            {
                SetProperty(ref _selectedPin, value);

                if (value == null) return;

                NavigationParameters keyValues = new NavigationParameters();
                keyValues.Add(nameof(SelectedPin), SelectedPin);

                _ = NavigationService.SelectTabAsync(nameof(Views.MapView), keyValues);
            }
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }

        private PinDataModel _selectedSearchPin;
        public PinDataModel SelectedSearchPin
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
            parameters.Add(nameof(_authentication.UserId), _authentication.UserId);
            await NavigationService.NavigateAsync($"{nameof(CreatePinView)}", parameters);
        }

        public ICommand SearchCommand { get; }
        private void SearchCommandRelease()
        {
            if (string.IsNullOrEmpty(SearchPin))
            {
                PinViewList = _mainList;
                return;
            }
            PinViewList = new List<PinViewModel>( 
                PinViewList.Where(x => x.Name.Contains(SearchPin) || 
                                         x.Description.Contains(SearchPin) || 
                                         x.Coordinate.Contains(SearchPin)).ToList());
        }

        public ICommand ExidCommand { get; }
        private void ExidCommandRelease()
        {
            NavigationService.NavigateAsync($"/{nameof(Views.StartPageView)}");
        }


        public ICommand LikeCommand { get; }
        private void LikeCommandRelease(object obj)
        {
            var model = PinViewList.First(p => p.Coordinate == obj.ToString());

            int index = PinViewList.IndexOf(model);
            PinViewList.Remove(model);
            model.IsVisable = !model.IsVisable;
            PinViewList.Insert(index, model);
            _pinManager.UpdateAsync(model.PinViewToPinData(_pinManager.GetAllPins(_authentication.UserId)));

            _mainList = PinViewList;
            PinViewList = new List<PinViewModel>(_mainList);
        }

        public ICommand DeleteCommand { get; }
        private void DeleteCommandRelease(object obj)
        {
            var model = PinViewList.First(p => p.Coordinate == (obj as PinViewModel).Coordinate);

            _pinManager.DeleteAsync(model.PinViewToPinData(_pinManager.GetAllPins(_authentication.UserId)));
            int index = PinViewList.IndexOf(model);
            PinViewList.Remove(model);
            _mainList = PinViewList = new List<PinViewModel>(PinViewList);

        }

        public ICommand EditCommand { get; }
        private void EditCommandRelease(object obj)
        {
            
            PinViewModel model;
            try
            {
                model = PinViewList.First(p => p.Coordinate == (obj as PinViewModel).Coordinate);
            }
            catch
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(UserMsg.ErrorEditPin,
                                                           UserMsg.Error,
                                                           UserMsg.Ok);
                return;
            }

            _isEditPin = true;

            NavigationParameters parametrs = new NavigationParameters();
            parametrs.Add(nameof(_isEditPin), true);
            parametrs.Add(nameof(PinViewModel), model);
            parametrs.Add(nameof(_authentication.UserId), _authentication.UserId);

            NavigationService.NavigateAsync(nameof(CreatePinView), parametrs);
        }

        public ICommand GoToSettingsCommand { get; }
        private void GoToSettingsCommandRelease()
        {
            NavigationService.NavigateAsync(nameof(SettingsView));
        }
        #endregion

        #region -- Override --
        public override void Initialize(INavigationParameters parameters)
        {
            PinViewList = _pinManager.GetAllPins(_authentication.UserId)
                                     .DataPinListToViewPinList();
            for (int i = 0; i < PinViewList.Count; i++)
            {
                BindCommnad(PinViewList[i]);
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            

            if (!string.IsNullOrEmpty(SearchPin))
            {
                SearchPin = string.Empty;
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            PinViewList = _pinManager.GetAllPins(_authentication.UserId)
                                     .DataPinListToViewPinList();
            

            if (PinViewList.Count == 0 || PinViewList[0].LikeCommand != null) return;

            for (int i = 0; i < PinViewList.Count; i++)
            {
                BindCommnad(PinViewList[i]);
            }

            _mainList = PinViewList;
        }

        #endregion

        #region -- Private --
        private bool _isEditPin;
        private List<PinViewModel> _mainList;

        private void BindCommnad(PinViewModel model)
        {
            model.LikeCommand = LikeCommand;
            model.EditCommand = EditCommand;
            model.DeleteCommand = DeleteCommand;
        }
        #endregion
    }
}
