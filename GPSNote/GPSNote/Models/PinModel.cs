﻿using System;
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
        public PinModel(string name, string description, Xamarin.Forms.Maps.Position position)
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
        public double Latitude
        {
            get => Position.Latitude;
            set
            {
                if(Position.Latitude != value)
                {
                    Position = new Xamarin.Forms.Maps.Position(value, Position.Longitude);
                }
            }
        }
        public double Longitude
        {
            get => Position.Longitude;
            set
            {
                if(position.Longitude != value)
                {
                    Position = new Xamarin.Forms.Maps.Position(Position.Latitude, value);
                }
            }
        }

        [Ignore]
        public string Coordinate { 
            get => $"{position.Latitude} {position.Longitude}";
        }
        [Ignore]
        public Xamarin.Forms.Maps.PinType  PinType { get; set; }
        [Ignore]
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
