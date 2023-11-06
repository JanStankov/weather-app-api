using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class WeatherController : Controller
    {
        private readonly IOpenWeatherService _openWeatherService;

        public WeatherController(IOpenWeatherService openWeatherService)
        {
            _openWeatherService = openWeatherService;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeatherCity(string city)
        {
            var userId = GetLoggedUserId();
            if (userId == null) return Unauthorized();

            var weatherList = await _openWeatherService.GetWeateherByCity(city);

            if(weatherList == null)
            {
                return BadRequest("City not found");
            }

            return Ok(weatherList);
        }

        private int? GetLoggedUserId()
        {
            
            var value = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return value == null ? null : int.Parse(value);
        }
    }
}
