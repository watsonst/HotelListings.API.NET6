using AutoMapper;
using HotelListings.Api.Data;
using HotelListings.Api.Models.Country;
using HotelListings.Api.Models.Hotel;
using HotelListings.Api.Models.Users;

namespace HotelListings.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig() //create maps between data types data and dto.
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //mapped in both directions
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        }
    }
}
