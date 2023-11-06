using WeatherAPI.Models;
using WeatherAPI.Models.Responses;

namespace WeatherAPI.Services
{
    public interface IOpenWeatherService
    {
        public Task<List<WeatherResponse>> GetWeateherByCity(string city);
    }
}
