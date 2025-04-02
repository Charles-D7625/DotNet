using System.Linq.Expressions;
using DotNetApplication.Data;
using DotNetApplication.Models;
using DotNetApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DotNetApplication.Repository;

public class VillaRepository : Repository<Villa>, IVillaRepository
{
    private readonly ApplicationDbContext _context;
    
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _context.Villas.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

}