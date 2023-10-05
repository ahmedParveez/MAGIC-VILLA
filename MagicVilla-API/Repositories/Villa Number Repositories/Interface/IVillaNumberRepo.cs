using MagicVilla_API.Models.Entities;
using MagicVilla_API.Repositories.Interfaces;

namespace MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Interface
{
    public interface IVillaNumberRepo : IRepo<VillaNumber>
    {
        Task<VillaNumber> UpdateNumberAsync(VillaNumber entity);
    }
}
