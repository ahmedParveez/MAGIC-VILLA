using MagicVilla_API.Models.Entities;
using MagicVilla_API.Repositories.Implementations;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Interface;

namespace MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Implementation
{
    public class VillaNumberRepo : Repo<VillaNumber>, IVillaNumberRepo
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<VillaNumber> UpdateNumberAsync(VillaNumber entity)
        {
            try
            {
                entity.UpdateDate = DateTime.Now.AddMonths(5);
                _db.VillaNumbers.Update(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
