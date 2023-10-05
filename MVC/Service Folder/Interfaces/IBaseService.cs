using MVC.Models;

namespace MVC.Service_Folder.Interfaces
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<Generic> SendAsync<Generic>(ApiRequest request);
    }
}
