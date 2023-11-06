using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using WeatherAPI.Models;
using WeatherAPI.Models.Responses;

namespace WeatherAPI.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public OpenWeatherService(IHttpClientFactory httpClientFactory,IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient();
            _mapper = mapper;
        }
        public async Task<List<WeatherResponse>> GetWeateherByCity(string city)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://api.openweathermap.org/geo/1.0/direct?q=" + city + "&limit=1&appid=a0c79e997288ad9c0e84bec6bc3338df");
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (apiResponse == "[]") {
                    return null;
                }
                JsonDocument apiResponseJson = JsonDocument.Parse(apiResponse);

                var firstElement = apiResponseJson.RootElement.EnumerateArray().First();

                var cityLat = firstElement.GetProperty("lat").GetDouble();
                var cityLon = firstElement.GetProperty("lon").GetDouble();

                HttpResponseMessage weatherResponse = await _httpClient.GetAsync(
                    "http://api.openweathermap.org/data/2.5/forecast?lat=" + cityLat + "&lon=" + cityLon + "&appid=a0c79e997288ad9c0e84bec6bc3338df&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    string apiweatherResponse = await weatherResponse.Content.ReadAsStringAsync();

                    WeatherDataResponse weatherDataResponse = JsonConvert.DeserializeObject<WeatherDataResponse>(apiweatherResponse);

                    var list = weatherDataResponse.List;

                    return _mapper.Map<List<WeatherResponse>>(list);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
public class WeatherDataResponse
{
    [JsonProperty("list") ]
    public List<WeatherData> List { get; set; }
}
