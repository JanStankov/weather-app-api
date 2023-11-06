using Microsoft.EntityFrameworkCore;
using WeatherAPI.Models;

namespace WeatherAPI.Database
{
    public class WeatherAPIDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public WeatherAPIDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
