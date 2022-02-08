using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GPSNote.Models
{
    public class PinModel
    {
        public PinModel()
        {

        }
        public PinModel(string address, string description, Xamarin.Forms.Maps.Position position)
        {
            Address = address;
            Description = description;
            Position = position;
        }
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        [Ignore]
        public string Coordinate { 
            get => $"{position.Latitude} {position.Longitude}";
        }
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
