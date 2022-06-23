using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Models
{
    public class PinDataModel : IEntity
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisable { get; set; } = true;
        public string Address { get; set; }
        public double Latitude
        {
            get; set;
        }
        public double Longitude
        {
            get; set;
        }
    }
}
