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
        public MainPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }
    }
}
