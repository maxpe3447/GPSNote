using GPSNote.Models.Weather;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Services.Weather
{
    public interface IWeatherService
    {
        WeatherModel GetWeatherInPosition(Position position);
    }
}
