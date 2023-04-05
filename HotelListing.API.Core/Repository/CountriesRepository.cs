using AutoMapper;
using HotelListings.Api.Data;
using HotelListings.API.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.API.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository //double inheritance. Can use any implementation between these two
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper) 
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {

            return await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
