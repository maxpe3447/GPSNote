﻿using GpsNote.Extensions;
using GPSNote.Models;
using GPSNote.Services.PinManager;
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
                                IPinManager repository) 
            : base(navigationService)
        {
            Title = Resources.TextControls.Pins;

            _PinManager = repository;

            CreatePinCommand = new Command(CreatePinCommandRelease);
            SearchCommand = new Command(SearchCommandRelease);
            LikeCommand = new Command(LikeCommandRelease);
            EditCommand = new Command(EditCommandRelease);
            DeleteCommand = new Command(DeleteCommandRelease);
            ExidCommand = new Command(ExidCommandRelease);
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

                if (value == null) return;

                NavigationParameters keyValues = new NavigationParameters();
                keyValues.Add(nameof(SelectedPin), SelectedPin);

                NavigationService.SelectTabAsync(nameof(Views.MapView), keyValues);
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
            await NavigationService.NavigateAsync($"{nameof(Views.CreatePinView)}", parameters);
        }

        public ICommand SearchCommand { get; }
        private void SearchCommandRelease()
        {
            if (string.IsNullOrEmpty(SearchPin))
            {
                PinModelsList = _mainList;
                return;
            }
            PinModelsList = new ObservableCollection<PinModel>( PinModelsList.Where(x => x.Name.Contains(SearchPin) || x.Description.Contains(SearchPin) || x.Coordinate.Contains(SearchPin)).ToList());
        }

        public ICommand ExidCommand { get; }
        private void ExidCommandRelease()
        {
            NavigationService.NavigateAsync($"/{nameof(Views.StartPageView)}");
        }


        public ICommand LikeCommand { get; }
        private void LikeCommandRelease(object obj)
        {
            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());

            int index = PinModelsList.IndexOf(model);
            PinModelsList.Remove(model);
            model.IsFavorit = !model.IsFavorit;
            PinModelsList.Insert(index, model);
            _PinManager.UpdateAsync(model);

            _mainList = PinModelsList;


        }

        public ICommand DeleteCommand { get; }
        private void DeleteCommandRelease(object obj)
        {
            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());
                
                _PinManager.DeleteAsync(model);
                int index = PinModelsList.IndexOf(model);
                PinModelsList.Remove(model);
            _mainList = PinModelsList;

        }

        public ICommand EditCommand { get; }
        private void EditCommandRelease(object obj)
        {

            var model = PinModelsList.First(p => p.Coordinate == obj.ToString());
            if (model != null)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert($"Edit: {model.Name}");
            }
            _mainList = PinModelsList;

        }
        #endregion

        #region -- Override --

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

                    BindCommnad(PinModelsList[PinModelsList.Count-1]);
                }
                else
                {
                    PinModelsList.Insert(index, newPinModel);

                    BindCommnad(PinModelsList[index]);
                    
                }
            }


            if (parameters.TryGetValue<int>(nameof(UserId), out var id))
            {
                UserId = id;
            }
            if (PinModelsList.Count == 0 || PinModelsList[0].LikeCommand != null) return;
            for (int i = 0; i < PinModelsList.Count; i++)
            {
                BindCommnad(PinModelsList[i]);
            }

            _mainList = PinModelsList;
        }

        #endregion

        #region -- Private --
        private int UserId { get; set; }
        private IPinManager _PinManager { get; }
        private ObservableCollection<PinModel> _mainList;

        private void BindCommnad(PinModel model)
        {
            model.LikeCommand = LikeCommand;
            model.EditCommand = EditCommand;
            model.DeleteCommand = DeleteCommand;
        }
        #endregion
    }
}
