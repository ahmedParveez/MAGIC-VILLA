using System.Linq.Expressions;

namespace MagicVilla_API.Repositories.Interfaces
{
    public interface IRepo<Generic_Class> where Generic_Class : class 
    {
        Task CreateAsync(Generic_Class model);
        Task RemoveAsync(Generic_Class model);
        Task<List<Generic_Class>> GetAllAsync(Expression<Func<Generic_Class, bool>>? filter = null,string? includeProperties = null);
        Task<Generic_Class> GetAsync(Expression<Func<Generic_Class,bool>>? filter = null,bool isTracked = true, string? includeProperties = null);
        Task SaveAsync();
    }
}
