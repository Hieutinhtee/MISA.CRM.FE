using MISA.CRM.CORE.Entities;

public interface IBaseService<T>
{
    Task<List<Customer>> GetAllAsync();

    Task<T> GetByIdAsync(Guid id);

    Task<List<T>> GetAllSortedAsync(string sortField, bool asc = true);

    Task<Guid> CreateAsync(T entity);

    Task<Guid> UpdateAsync(Guid id, T entity);

    Task<int> DeleteAsync(Guid id);
}