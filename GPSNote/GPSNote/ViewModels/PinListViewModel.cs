using GpsNote.Extensions;
using GPSNote.Extansion;
using GPSNote.Models;
using GPSNote.Resources;
using GPSNote.Services.Authentication;
using GPSNote.Services.PinManager;
using GPSNote.Services.Repository;
using GPSNote.Views;
using Prism.Commands;
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
        private bool _isEditPin;
        private List<PinViewModel> _mainList;
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
            set => SetProperty(ref _selectedPin, (value != null) ?value : _selectedPin/*, OnChangeSelectedPin*/);
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
            set => SetProperty(ref _selectedSearchPin, value);
        }

        private ICommand createPinCommand;
        public ICommand CreatePinCommand { get => createPinCommand ?? new DelegateCommand(CreatePinCommandRelease); }

        private ICommand searchCommand;
        public ICommand SearchCommand { get => searchCommand ?? new DelegateCommand(SearchCommandRelease); }

        private ICommand exidCommand;
        public ICommand ExidCommand { get => exidCommand ?? new DelegateCommand(ExidCommandRelease); }

        private ICommand likeCommand;
        public ICommand LikeCommand { get => likeCommand ?? new Command(LikeCommandRelease); }

        private ICommand deleteCommand;
        public ICommand DeleteCommand { get => deleteCommand ?? new Command(DeleteCommandRelease); }

        private ICommand editCommand;
        public ICommand EditCommand { get => editCommand ?? new Command(EditCommandRelease); }

        private ICommand goToSettingsCommand;
        public ICommand GoToSettingsCommand { get => goToSettingsCommand ?? new DelegateCommand(GoToSettingsCommandRelease); }

        private ICommand itemTappedCommand;
        public ICommand ItemTappedCommand { get => itemTappedCommand ?? new DelegateCommand(ItemTappedCommandRelease); }

        #endregion

        #region -- Override --
        public override void Initialize(INavigationParameters parameters)
        {
            PinViewList = _pinManager.GetAllPins()
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

            PinViewList = _pinManager.GetAllPins()
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
        private void BindCommnad(PinViewModel model)
        {
            model.LikeCommand = LikeCommand;
            model.ItemTappedCommand = ItemTappedCommand;
            model.EditCommand = EditCommand;
            model.DeleteCommand = DeleteCommand;
        }
        private async void CreatePinCommandRelease()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(_authentication.UserId), _authentication.UserId);
            await NavigationService.NavigateAsync($"{nameof(CreatePinView)}", parameters);
        }

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

        private void ExidCommandRelease()
            => NavigationService.NavigateAsync($"/{nameof(Views.StartPageView)}");

        private void LikeCommandRelease(object obj)
        {
            var model = PinViewList.First(p => p.Coordinate == obj.ToString());

            int index = PinViewList.IndexOf(model);
            PinViewList.Remove(model);
            model.IsVisable = !model.IsVisable;
            PinViewList.Insert(index, model);
            _pinManager.UpdateAsync(model.PinViewToPinData(_pinManager.GetAllPins()));

            _mainList = PinViewList;
            PinViewList = new List<PinViewModel>(_mainList);
        }

        private void DeleteCommandRelease(object obj)
        {
            var model = PinViewList.First(p => p.Coordinate == (obj as PinViewModel).Coordinate);

            _pinManager.DeleteAsync(model.PinViewToPinData(_pinManager.GetAllPins()));
            int index = PinViewList.IndexOf(model);
            PinViewList.Remove(model);
            _mainList = PinViewList = new List<PinViewModel>(PinViewList);
        }
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

        private void GoToSettingsCommandRelease()
            => NavigationService.NavigateAsync(nameof(SettingsView));

        private void ItemTappedCommandRelease()
        {
            if (_selectedPin == null) return;

            SelectedPin.IsVisable = true;
            _pinManager.UpdateAsync(SelectedPin.PinViewToPinData(_pinManager.GetAllPins()));

            NavigationParameters keyValues = new NavigationParameters();
            keyValues.Add(nameof(SelectedPin), SelectedPin);

            _ = NavigationService.SelectTabAsync(nameof(MapView), keyValues);
        }
        #endregion
    }
}
