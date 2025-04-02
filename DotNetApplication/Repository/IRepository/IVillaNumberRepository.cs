using System.Linq.Expressions;
using DotNetApplication.Models;

namespace DotNetApplication.Repository.IRepository;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateNumberAsync(VillaNumber entity);
}