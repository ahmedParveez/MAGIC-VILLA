using MagicVilla_API.Models.Entities;

namespace MagicVilla_API.Repositories.Interfaces
{
    public interface IVillaRepo : IRepo<Villa>
    {
        Task<Villa> UpdateAsync(Villa model);
    }
}
