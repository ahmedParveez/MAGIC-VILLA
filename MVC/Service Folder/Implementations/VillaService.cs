using MagicVilla_Utilities;
using MVC.Models;
using MVC.Models.DTO;
using MVC.Service_Folder.Interfaces;

namespace MVC.Service_Folder.Implementations
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _ClientFactory;
        private string _filePath;
        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            _ClientFactory = httpClient;
            _filePath = configuration.GetValue<string>("ServiceUrls:VillaUrl");
        }

        public Task<Generic> DeleteAsync<Generic>(int Id)
        {
            return SendAsync<Generic>(new ApiRequest()
            {
                apiType = StaticDetails.ApiType.DELETE,
                //Data = ,
                Url = _filePath + "/api/VillaAPI/" + Id
            });
        }

        public Task<Generic> GetAllAsync<Generic>()
        {
            return SendAsync<Generic>(new ApiRequest()
            {
                apiType = StaticDetails.ApiType.GET,
                //Data = ,
                Url = _filePath + "/api/VillaAPI"
            });
        }

        public Task<Generic> GetAsync<Generic>(int Id)
        {
            return SendAsync<Generic>(new ApiRequest()
            {
                apiType = StaticDetails.ApiType.GET,
                //Data = ,
                Url = _filePath + "/api/VillaAPI/" + Id
            });
        }

        public Task<Generic> PostAsync<Generic>(CreateDTO create)
        {
            return SendAsync<Generic>(new ApiRequest()
            {
                apiType = StaticDetails.ApiType.POST,
                Data = create,
                Url = _filePath + "/api/VillaAPI" 
            });
        }

        public Task<Generic> PutAsync<Generic>(UpdateDTO update)
        {
            return SendAsync<Generic>(new ApiRequest()
            {
                apiType = StaticDetails.ApiType.PUT,
                Data = update,
                Url = _filePath + "/api/VillaAPI/" + update.Id
            });
        }
    }
}
