using HotelListings.Api.Data;

namespace HotelListings.Api.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country> //can't use generic here, must use one of the classes
    {
        Task<Country> GetDetails(int id);
    }
}
