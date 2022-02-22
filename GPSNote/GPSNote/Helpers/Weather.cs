using GPSNote.Models.Weather;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace GPSNote.Helpers
{
    public static class Weather
    {
        static public WeatherModel GetResponse(double Latitude, double Longitude) {

            string uri = $"http://api.openweathermap.org/data/2.5/forecast?lat={Latitude}&lon={Longitude}&units=metric&cnt=4&appid=c49f31221dcddd61a6992ae7d5810937";            

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using(StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<WeatherModel>(response);
        }
    }
}
