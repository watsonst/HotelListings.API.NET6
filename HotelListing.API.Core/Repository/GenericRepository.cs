using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListings.API.Core.Contracts;
using HotelListings.Api.Data;
using HotelListings.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using HotelListings.API.Core.Exceptions;

namespace HotelListings.API.Core.Repository
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

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)//sending source DTO to add, returning DTO. Use generics Tsource/Tresult because we don't know if it is a country or a hotel
        {
            var entity = _mapper.Map<T>(source);//Doesn't know what T is right now. We spesify in individual repos. Mapping source to T entity

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
            //May say that is a lot of mapping. Depends on our needs. Ok to expose/return entire entity? Or do we want to only map to our DTO fields and return dto
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
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
            return await _context.Set<T>().ToListAsync(); //get db set of type T. ToList is executing command
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

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context.Set<T>()
                .ProjectTo<TResult>( _mapper.ConfigurationProvider)
                .ToListAsync();//build query before executable code. 
        }

        public async Task<T> GetAsync(int? id)
        {
           if (id == null)
           {
                return null;
           }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<TResult?> GetAsync<TResult>(int? id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result == null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");// if (?) id has value return id else (:) return no key provided
            }

            return _mapper.Map<TResult>(result);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource, TResult>(int id, TSource source)
        {
            var entity = await GetAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _mapper.Map(source, entity);//doing this mapping here removes the need for mapping in the individual controllers. DRY
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
