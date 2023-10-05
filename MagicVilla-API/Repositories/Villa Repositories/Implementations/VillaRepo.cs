using MagicVilla_API.Models.Entities;
using MagicVilla_API.Repositories.Interfaces;
using MagicVilla_VillaAPI.Data;

namespace MagicVilla_API.Repositories.Implementations
{
    public class VillaRepo : Repo<Villa>, IVillaRepo
    {
        private ApplicationDbContext _db;
        public VillaRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Villa> UpdateAsync(Villa model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                _db.Update(model);
                await _db.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                 throw new Exception(ex.Message);
            }
           
        }
    }
}
