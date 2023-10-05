using MagicVilla_Utilities;
using MVC.Models;
using MVC.Service_Folder.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MVC.Service_Folder.Implementations
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            this.responseModel = new();
        }
        public async Task<Generic> SendAsync<Generic>(ApiRequest request)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicVilla");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.Url);
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
                        Encoding.UTF8, "application/json");

                }
                switch (request.apiType)
                {
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == HttpStatusCode.NotFound ||
                         apiResponse.StatusCode == HttpStatusCode.BadRequest))
                    {
                        ApiResponse.StatusCode = HttpStatusCode.BadGateway;
                        ApiResponse.isSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<Generic>(res);
                        return returnObj;
                    }
                }
                catch (Exception ex)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<Generic>(apiContent);
                    return exceptionResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<Generic>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse()
                {
                    isSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };
                var res = JsonConvert.SerializeObject(response);
                var returnObj = JsonConvert.DeserializeObject<Generic>(res);
                return returnObj;
            }
        }
    }
}
