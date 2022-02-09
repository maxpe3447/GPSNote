using GPSNote.Models;
using GPSNote.Services.Repository;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        public PinListViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            Title = "Pins List";
            CreatePinCommand = new Command(CreatePinCommandRelease);
        }
        #region -- Properties --
        private ObservableCollection<PinModel> _pinsList;
        public ObservableCollection<PinModel> PinsList
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
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

                NavigationService.SelectTabAsync("MapView", keyValues);
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
        #endregion

        #region -- Override --

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (parameters.ContainsKey(nameof(PinsList)))
            {
                parameters.Add(nameof(this.PinsList), PinsList);
               
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<ObservableCollection<PinModel>>(nameof(this.PinsList), out var newCounterValue))
            {
                PinsList = newCounterValue;
            }

            if(parameters.TryGetValue<PinModel>(nameof(PinModel), out var newPinModel))
            {
                PinsList.Add(newPinModel);
            }
            if (parameters.TryGetValue<int>(nameof(UserId), out var id))
            {
                UserId = id;
            }
        }
        #endregion

        #region -- Private --
        private int UserId { get; set; }
        #endregion
    }
}
