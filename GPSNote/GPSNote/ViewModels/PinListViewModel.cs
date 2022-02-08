using GPSNote.Models;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace GPSNote.ViewModels
{
    public class PinListViewModel : ViewModelBase
    {
        public PinListViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pins List";
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
        }
        #endregion
    }
}
