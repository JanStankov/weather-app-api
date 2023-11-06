using WeatherAPI.Models;
using WeatherAPI.Models.Commands;

namespace WeatherAPI.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(CreateUserCommand createUserCommand);

        public Task<User> GetUserByEmail(string email);
    }
}
