using HotelListings.Api.Contracts;
using HotelListings.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.Api.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository //double inheritance. Can use any implementation between these two
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context) : base(context) 
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {

            return await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
