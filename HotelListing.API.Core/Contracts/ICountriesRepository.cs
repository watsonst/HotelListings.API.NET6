using HotelListings.Api.Data;
using HotelListings.API.Core.Models.Country;

namespace HotelListings.API.Core.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country> //can't use generic here, must use one of the classes
    {
        Task<CountryDto> GetDetails(int id);
    }
}
