// Path: Core/Interfaces/IGenericRepository.cs
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Basic CRUD (keep if you already have implementations)
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        // ---- Specification-enabled methods (entity shape) ----
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        // ---- Specification-enabled methods (projection shape) ----
        Task<TResult?> GetEntityWithSpecAsync<TResult>(IProjectionSpecification<T, TResult> spec);
        Task<IReadOnlyList<TResult>> ListAsync<TResult>(IProjectionSpecification<T, TResult> spec);
        Task<int> CountAsync(ISpecification<T> spec); 
    }
}
