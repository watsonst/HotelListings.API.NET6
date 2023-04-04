using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListings.Api.Contracts;
using HotelListings.Api.Data;
using HotelListings.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListings.Api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class //inhert from contract IGenRepo
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .ToListAsync(); //get db set of type T. ToList is executing command
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)//T represents the model. TResult represents DTO
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                .Skip(queryParameters.StartIndex)//skipping to start index that comes in the with query params. Start at n number
                .Take(queryParameters.PageSize)//the number of records to take/display per page
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)//looks to configs, mapperConfig.cs
                .ToListAsync();
            
            return new PagedResult<TResult> 
            {
                Items = items,//list of items returned for that page
                PageNumber = queryParameters.PageNumber,//the page user is on
                RecordNumber = queryParameters.PageSize,//number of items on that page
                TotalCount = totalSize
            };

        }

        public async Task<T> GetAsync(int id)
        {
           if (id == null)
           {
                return null;
           }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
