using GPSNote.Models;
using GPSNote.Resources;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Extansion
{
    public static class PinModelExtansion
    {
        public static PinViewModel PinDataToPinView(this PinDataModel pinDataModel)
            => new PinViewModel
            {
                Name = pinDataModel.Name,
                Description = pinDataModel.Description,
                Address = pinDataModel.Address,
                IsVisable = pinDataModel.IsVisable,
                Position = new Position(pinDataModel.Latitude, pinDataModel.Longitude)
            };

        public static PinDataModel PinViewToPinData(this PinViewModel pinViewModel, int userId)
        {
            return new PinDataModel
            {
                Name = pinViewModel.Name,
                Address = pinViewModel.Address,
                Description = pinViewModel.Description,
                IsVisable = pinViewModel.IsVisable,
                Latitude = pinViewModel.Position.Latitude,
                Longitude = pinViewModel.Position.Longitude,
                UserId = userId
            };
        }

        public static PinDataModel PinViewToPinData(this PinViewModel pinViewModel, List<PinDataModel> pinDatas)
        {

            var pinData = pinDatas.Where(x => x.Latitude == pinViewModel.Position.Latitude && x.Longitude == pinViewModel.Position.Longitude).First();

            return new PinDataModel
               {
                   Name = pinViewModel.Name,
                   Address = pinViewModel.Address,
                   Description = pinViewModel.Description,
                   IsVisable = pinViewModel.IsVisable,
                   Latitude = pinViewModel.Position.Latitude,
                   Longitude = pinViewModel.Position.Longitude,
                   UserId = pinData.UserId,
                   Id = pinData.Id
               };
        }

        public static void RestPins(this IList<Pin> oldPins, IList<PinViewModel> newPins)
        {
            oldPins.Clear();

            foreach (var pin in newPins)
            {
                if (!pin.IsVisable)
                {
                    continue;
                }
                oldPins.Add(new Pin
                {
                     Label = pin.Name,
                     Position = pin.Position,
                    Icon = BitmapDescriptorFactory.FromView(
                            new Controls.BindingPinIconView((ImageSource)App.Current
                                                                            .Resources[ImageNames.ic_placeholder]))
                });
            }
        }

        public static List<PinViewModel> DataPinListToViewPinList(this List<PinDataModel> dataPins)
        {
            var viewPins = new List<PinViewModel>();
            foreach (var pin in dataPins)
            {
                viewPins.Add(pin.PinDataToPinView());
            }
            return viewPins;
        }
    }
}
