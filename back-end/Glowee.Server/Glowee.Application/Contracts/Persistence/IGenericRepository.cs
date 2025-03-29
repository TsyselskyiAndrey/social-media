using Glowee.Domain.Common;

namespace Glowee.Application.Contracts.Persistence;

public interface IGenericRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    Task<IReadOnlyList<TEntity>> GetAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}