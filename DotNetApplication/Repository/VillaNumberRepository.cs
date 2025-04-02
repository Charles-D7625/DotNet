using System.Linq.Expressions;
using DotNetApplication.Data;
using DotNetApplication.Models;
using DotNetApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DotNetApplication.Repository;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext _context;
    
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<VillaNumber> UpdateNumberAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _context.VillaNumbers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

}