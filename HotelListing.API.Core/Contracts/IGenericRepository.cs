using HotelListings.API.Core.Models;

namespace HotelListings.API.Core.Contracts
{
    public interface IGenericRepository<T> where T : class //enforces what should happen. class = country/hotel
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);//introduce another generic. Use to reduce some mapping in controler and move to centralized place
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> Exists(int id);

    }
}
