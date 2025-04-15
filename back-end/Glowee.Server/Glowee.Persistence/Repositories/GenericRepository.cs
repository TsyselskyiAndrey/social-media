using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Common;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

/// <summary>
/// Generic class for all Db repositories
/// </summary>
/// <typeparam name="TEntity">Entity of repository</typeparam>
/// <typeparam name="TKey">Identifier of entity</typeparam>
public class GenericRepository<TEntity, TKey> :
    IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected readonly SqlDbContext _context;

    public GenericRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync()
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(q => q.Id!.Equals(id));
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TKey key)
    {
        var trackedEntity = await _context.Set<TEntity>().FindAsync(key);
        if (trackedEntity is not null)
        {
            _context.Remove(trackedEntity);
            await _context.SaveChangesAsync();
        }
    }
}