using WeatherAPI.Models;
using WeatherAPI.Models.Commands;
using WeatherAPI.Models.Responses;

namespace WeatherAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<(bool success, LoginResponse content)> Login(string email, string password);
        public Task<RegisterResponse> Register(CreateUserCommand user);
    }
}
