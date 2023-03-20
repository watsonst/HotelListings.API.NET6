using HotelListings.Api.Contracts;
using HotelListings.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.Api.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;

        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
