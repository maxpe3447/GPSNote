using GPSNote.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSNote.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        public MapViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Map Page";

            MapClickCommand = new Command(MapClickCommandRelease);
            PinsList = new ObservableCollection<PinModel>
            {
                new PinModel("Addres1", "1", new Position(47.8431096, 35.0874433)),
                new PinModel("Addres2", "2", new Position(47.846540, 35.087064)),
                new PinModel("Addres3", "3", new Position(47.838393, 35.098817))
            };
            
        }

        #region -- Command -- 
        public ICommand MapClickCommand { get; }
        private void MapClickCommandRelease()
        {
            System.Diagnostics.Debug.WriteLine($"MapClick: !!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        #endregion

        public ObservableCollection<PinModel> PinsList { get; }

        public override void Initialize(INavigationParameters parameters)
        {
            
        }
    }
}
