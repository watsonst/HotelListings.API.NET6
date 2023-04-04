using AutoMapper;
using HotelListings.Api.Contracts;
using HotelListings.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.Api.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
