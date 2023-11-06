using AutoMapper;
using WeatherAPI.Models;
using WeatherAPI.Models.Responses;

namespace WeatherAPI.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, RegisterResponse>();
            CreateMap<WeatherData, WeatherResponse>().ForMember(dest => dest.Precipitation, opt => opt.MapFrom(src => src.pop))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.dt_txt));
        }
    }
}
