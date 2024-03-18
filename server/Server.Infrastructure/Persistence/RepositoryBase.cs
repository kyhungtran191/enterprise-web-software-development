using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Interfaces.Persistence;

namespace Server.Infrastructure.Persistence;

public class RepositoryBase<T, Key> : IRepository<T, Key> where T : class
{
    private readonly DbSet<T> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsEnumerable();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Key id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}