using AutoMapper;
using HotelListings.Api.Data;
using HotelListings.Api.Models.Country;
using HotelListings.Api.Models.Hotel;

namespace HotelListings.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig() //create maps between data types data and dto.
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //mapped in both directions
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country,CountryDto>().ReverseMap();
            CreateMap<Country,UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
