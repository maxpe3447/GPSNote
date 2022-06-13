using GPSNote.Models.Weather;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        public WeatherModel GetWeatherInPosition(Position position)
        {
            string uri = $"http://api.openweathermap.org/data/2.5/forecast?lat={position.Latitude}&lon={position.Longitude}&units=metric&cnt=4&appid=c49f31221dcddd61a6992ae7d5810937";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<WeatherModel>(response);
        }
    }
}
