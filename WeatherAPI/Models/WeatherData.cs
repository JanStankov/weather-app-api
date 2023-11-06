using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WeatherAPI.Models
{

    public class WeatherData
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Wind Wind { get; set; }
        public double pop { get; set; }
        public string dt_txt { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Icon { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
    }

    public class WeatherDataResponse
    {
        public List<WeatherData> List { get; set; }
    }
}
