using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Models
{
    public class PinViewModel
    {
        
        private Position position;

        #region -- Public --
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool IsVisable { get; set; } = true;
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
        public string Coordinate
        {
            get => $"{position.Latitude:0.00000} {position.Longitude:0.000000}";
        }

        public ICommand LikeCommand { get; set; } = null;
        public ICommand ItemTappedCommand { get; set; } = null;
        public ICommand DeleteCommand { get; set; } = null;
        public ICommand EditCommand { get; set; } = null;

        public PinType PinType { get; set; }
        #endregion

        #region -- Overrides
        public override string ToString()
        {
            return $"{Name}\n{Description}\n{Coordinate}";
        }
        #endregion

    }
}
