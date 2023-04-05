using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListings.Api.Data;
using HotelListings.API.Core.Contracts;
using HotelListings.API.Core.Exceptions;
using HotelListings.API.Core.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.API.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository //double inheritance. Can use any implementation between these two
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper) 
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<CountryDto> GetDetails(int id)
        {
            var country = await _context.Countries.Include(q => q.Hotels)
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }

            return country;
        }
    }
}
