namespace HotelListings.Api.Contracts
{
    public interface IGenericRepository<T> where T : class //enforces what should happen. class = country/hotel
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> Exists(int id);

    }
}
