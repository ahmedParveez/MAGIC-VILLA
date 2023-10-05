using MVC.Models.DTO;

namespace MVC.Service_Folder.Interfaces
{
    public interface IVillaService
    {
        Task<Generic> GetAllAsync<Generic>();
        Task<Generic> GetAsync<Generic>(int Id);
        Task<Generic> PostAsync<Generic>(CreateDTO create);
        Task<Generic> PutAsync<Generic>(UpdateDTO update);
        Task<Generic> DeleteAsync<Generic>(int Id);
    }
}
