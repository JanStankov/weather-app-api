namespace WeatherAPI.Models.Responses
{
    public class WeatherResponse
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Wind Wind { get; set; }
        public double Precipitation { get; set; }
        public string DateTime { get; set; }
        
    }
}
