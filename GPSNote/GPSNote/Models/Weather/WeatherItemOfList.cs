using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Models.Weather
{
    public class WeatherItemOfList
    {
        public WeatherMain Main { get; set; }
        public List<WeatherWeat> weather { get; set; }
    }
}
