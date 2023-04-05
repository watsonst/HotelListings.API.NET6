using HotelListings.API.Core.Models;
using Microsoft.Build.Execution;

namespace HotelListings.API.Core.Contracts
{
    public interface IGenericRepository<T> where T : class //enforces what should happen. class = country/hotel
    {
        Task<T> GetAsync(int? id);//T respresents entity- we don't want the entity exposed to those who call the api
        Task<TResult?> GetAsync<TResult>(int? id);
        Task<List<T>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);//introduce another generic. Use to reduce some mapping in controler and move to centralized place
        Task<T> AddAsync(T entity);
        Task<TResult> AddAsync<TSource, TResult>(TSource source);//want dto to come into the repo. tsource = paramater tresult = expected return type
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task UpdateAsync<TSource>(int id, TSource source);//get source(dto object) and take id
        Task<bool> Exists(int id);

    }
}
