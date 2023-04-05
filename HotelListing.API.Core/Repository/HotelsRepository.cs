using AutoMapper;
using HotelListings.API.Core.Contracts;
using HotelListings.Api.Data;

namespace HotelListings.API.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
