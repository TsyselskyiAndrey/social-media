using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Common;
using Microsoft.EntityFrameworkCore;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

/// <summary>
/// Generic class for all Db repositories
/// </summary>
/// <typeparam name="TEntity">Entity of repository</typeparam>
/// <typeparam name="TKey">Identifier of entity</typeparam>
public class GenericRepository<TEntity, TKey> : 
    IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected readonly SqlDbContext Context;

    public GenericRepository(SqlDbContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        Context.Remove(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
}