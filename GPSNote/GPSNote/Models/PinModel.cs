using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Models
{
    public class PinModel
    {
        public PinModel(string address, string description, Xamarin.Forms.Maps.Position position)
        {
            Address = address;
            Description = description;
            Position = position;
        }

        public double Latitude { get => position.Latitude; }
        public double Longitude { get => position.Longitude; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Xamarin.Forms.Maps.PinType  PinType { get; set; }
        public Xamarin.Forms.Maps.Position Position
        {
            get => position;
            set
            {
                if (!position.Equals(value))
                {
                    position = value;
                }
            }
        }
        #region -- Private --
        private Xamarin.Forms.Maps.Position position;
        #endregion
    }
}
