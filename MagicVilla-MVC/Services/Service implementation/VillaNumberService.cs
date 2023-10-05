using MagicVilla_MVC.Models;
using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using MagicVilla_MVC.Models.VM;
using MagicVilla_MVC.Services.IServices;
using MagicVilla_Utilities;
using MagicVilla_Web.Services;

namespace MagicVilla_MVC.Services.Service_implementation
{
	public class VillaNumberService : BaseService, IVillaNumberService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private string _villaNumberUrl;
        public VillaNumberService(IHttpClientFactory httpClientFactory,IConfiguration configuration) :base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
			_villaNumberUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

		}
        public Task<T> CreateAsync<T>(CreateNumberDTO dto)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = StaticDetails.ApiType.POST,
				Data = dto,
				Url = _villaNumberUrl + "/api/VillaNumber",
			});
		}

        public Task<T> DeleteAsync<T>(int id)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = StaticDetails.ApiType.DELETE,
				//Data = dto,
				Url = _villaNumberUrl + "/api/VillaNumber/" +id,
			});
		}

		public Task<T> GetAllAsync<T>()
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = StaticDetails.ApiType.GET,
				//Data = dto,
				Url = _villaNumberUrl + "/api/VillaNumber",
			});
		}

		public Task<T> GetAsync<T>(int id)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = StaticDetails.ApiType.GET,
				//Data = dto,
				Url = _villaNumberUrl + "/api/VillaNumber/" +id,
			});
		}

		public Task<T> UpdateAsync<T>(UpdateNumberDTO dto)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = StaticDetails.ApiType.PUT,
				Data = dto,
				Url = _villaNumberUrl + "/api/VillaNumber/" +dto.villaNo,
			});
		}
	}
}
