using GPSNote.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Navigation.TabbedPages;

namespace GPSNote.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        //public ObservableCollection<PinModel> PinsList { get; }
        //async void SelectTab(object parameters)
        //{
        //    var result = await NavigationService.SelectTabAsync(nameof(Views.PinListView));
        //}

    }
}
