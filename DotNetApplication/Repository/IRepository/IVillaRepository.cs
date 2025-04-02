using System.Linq.Expressions;
using DotNetApplication.Models;

namespace DotNetApplication.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}