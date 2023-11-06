using WeatherAPI.Database.Repositories.Interfaces;
using WeatherAPI.Models.Commands;
using WeatherAPI.Models;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Utilities.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net.Mail;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;

namespace WeatherAPI.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private WeatherAPIDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(WeatherAPIDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<User> CreateUser(CreateUserCommand createUserCommand)
        {
            var passwordHash = _passwordHasher.Hash(createUserCommand.Password);

            if (!IsValidEmail(createUserCommand.Email)) {
                return null;
            }

            if (createUserCommand.Name.IsNullOrEmpty() || createUserCommand.Surname.IsNullOrEmpty() || createUserCommand.Password.IsNullOrEmpty()) {
                return null;
            }

            var exists = await _context.Users.Where(x => x.Email == createUserCommand.Email).FirstOrDefaultAsync();

            if (exists != null) {
                return null;
            }

            var user = new User
            {
                Name = createUserCommand.Name,
                Surname = createUserCommand.Surname,
                Email = createUserCommand.Email,
                PasswordHash = passwordHash

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            return user;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}