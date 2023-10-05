using MagicVilla_API.Repositories.Interfaces;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositories.Implementations
{
    public class Repo<Generic_Class> : IRepo<Generic_Class> where Generic_Class : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<Generic_Class> dbSet;
        public Repo(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<Generic_Class>();
            // Below is the syntax for including the properties of villa
            // Syntax : _db.VillaNumbers.Include(includePropertiesOf => includePropertiesOf.Villa).ToListAsync();
        }

        // Create
        public async Task CreateAsync(Generic_Class model)
        {
            try
            {
                await dbSet.AddAsync(model);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get all
        public async Task<List<Generic_Class>> GetAllAsync(Expression<Func<Generic_Class, bool>> filter = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<Generic_Class> Query = dbSet;
                
                // Filter in Action
                if(filter != null)
                {
                    Query = Query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Query = Query.Include(property);
                    }
                }
                return await Query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Generic_Class> GetAsync(Expression<Func<Generic_Class, bool>> filter = null, bool isTracked = true, string? includeProperties = null)
        {
            try
            {
                IQueryable<Generic_Class> Query = dbSet;

                // As No tracking in Action

                //if (isTracked) //<-- Try this
                if (!isTracked)
                {
                    Query = Query.AsNoTracking();
                }

                // Filter in Action
                if (filter != null)
                {
                    Query = Query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Query = Query.Include(property);
                    }
                }
                return await Query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveAsync(Generic_Class model)
        {
            try
            {
                dbSet.Remove(model);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
