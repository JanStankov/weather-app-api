using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPI.Database.Repositories.Interfaces;
using WeatherAPI.Models.Commands;
using WeatherAPI.Models.Responses;
using WeatherAPI.Services.Interfaces;
using WeatherAPI.Utilities.Interfaces;
using WeatherAPI.Models;
using AutoMapper;

namespace WeatherAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Settings _settings;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher,
             IMapper mapper, Settings settings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _settings = settings;
        }

        public async Task<(bool success, LoginResponse content)> Login(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return (false, new LoginResponse(null, null,null,null));
            }

            if (!_passwordHasher.Verify(user.PasswordHash, password))
            {
                return (false, new LoginResponse(null, null, null, null));
            }

            return (true, new LoginResponse(GenerateJwtToken(AssembleClaimsIdentity(user)), user.Email, user.Name, user.Surname));
        }

        public async Task<RegisterResponse> Register(CreateUserCommand user)
        {
            var newUser = await _userRepository.CreateUser(user);

            return _mapper.Map<RegisterResponse>(newUser);

        }

        private ClaimsIdentity AssembleClaimsIdentity(User user)
        {
            var subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Email, user.Email),
        });
            return subject;
        }

        private string GenerateJwtToken(ClaimsIdentity subject)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
