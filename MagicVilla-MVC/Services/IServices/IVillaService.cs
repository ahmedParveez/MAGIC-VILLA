using MagicVilla_MVC.Models.DTO;

namespace MagicVilla_MVC.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CreateDTO dto);
        Task<T> UpdateAsync<T>(UpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
