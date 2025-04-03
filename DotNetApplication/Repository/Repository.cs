using System.Linq.Expressions;
using DotNetApplication.Data;
using DotNetApplication.Models;
using DotNetApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DotNetApplication.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> DbSet;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        //_context.VillaNumbers.Include(v => v.Villa).ToList();
        this.DbSet = _context.Set<T>();
    }
    
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        
        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracking = true, string? includeProperties = null)
    {
        IQueryable<T> query = DbSet;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        DbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}