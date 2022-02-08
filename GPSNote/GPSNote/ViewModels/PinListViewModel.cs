using GPSNote.Models;
using Prism.Navigation;
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
            //PinsList = new ObservableCollection<PinModel>
            //{
            //    new PinModel("Addres1", "1", new Position(47.8431096, 35.0874433)),
            //    new PinModel("Addres2", "2", new Position(47.846540, 35.087064)),
            //    new PinModel("Addres3", "3", new Position(47.838393, 35.098817))
            //};
        }
        #region -- Properties --
        public ObservableCollection<PinModel> _pinsList;
        public ObservableCollection<PinModel> PinsList
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
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
