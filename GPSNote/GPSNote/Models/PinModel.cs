using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Models
{
    public class PinModel : IEntity
    {
        public PinModel()
        {

        }
        public PinModel(string name, string description, Position position)
        {
            Name = name;
            Description = description;
            Position = position;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFavorit { get; set; } = false;
        public double Latitude
        {
            get => Position.Latitude;
            set
            {
                if (Position.Latitude != value)
                {
                    Position = new Position(value, Position.Longitude);
                }
            }
        }
        public double Longitude
        {
            get => Position.Longitude;
            set
            {
                if (position.Longitude != value)
                {
                    Position = new Position(Position.Latitude, value);
                }
            }
        }
        public string Address { get; set; }
        [Ignore]
        public string Coordinate { 
            get => $"{position.Latitude:0.00000} {position.Longitude:0.000000}";
        }
        [Ignore]
        public PinType PinType { get; set; }
        [Ignore]
        public Position Position
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
        [Ignore]
        public ICommand LikeCommand { get; set; } = null;
        [Ignore]
        public ICommand EditCommand { get; set; } = null;
        [Ignore]
        public ICommand DeleteCommand { get; set; } = null;
        #region -- Overrides
        public override string ToString()
        {
            return $"{Name}\n{Description}\n{Coordinate}";
        }
        #endregion

        #region -- Private --
        private Position position;
        #endregion
    }
}
