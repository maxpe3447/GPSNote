using GPSNote.Models;
using Prism.Navigation;
using System.Linq;
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

        #region -- Properties -- 
        public ObservableCollection<PinModel> PinsList { get; }

        static private Map mapPins;

        static public Map MapPins
        {
            get => mapPins;
            set
            {
                mapPins = value;//SetProperty(ref mapPins, value); 
                //mapPins.MapClicked += MapPins_MapClicked;
                

            }
        }

        
        #endregion


        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(PinModel.UserId)))
            {
                UserId = parameters.GetValue<int>(nameof(PinModel.UserId));
            }

            mapPins.MapClicked += (s, e) =>
            {
               // if (PinsList.Where(x => x.Position == e.Position).Count() > 0)
                {
                    mapPins.MoveToRegion(MapSpan.FromCenterAndRadius(PinsList[1].Position, Distance.FromKilometers(1)));
                }
                //else
                   // Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Test");
            };
        }

        #region -- Private --
        private int UserId { get; set; }

        #endregion
    }
}
