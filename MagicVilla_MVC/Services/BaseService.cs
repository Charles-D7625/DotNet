using System.Text;
using MagicVilla_MVC.Models;
using MagicVilla_MVC.Services.IServices;
using MagicVilla_Utility;
using Newtonsoft.Json;

namespace MagicVilla_MVC.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    
    public IHttpClientFactory httpClient { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {
        this.httpClient = httpClient;
        this.responseModel = new APIResponse();
    }
    
    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }
            switch (apiRequest.ApiType)
            {
                case SD.ApiType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
            }

            HttpResponseMessage apiResponse = await client.SendAsync(message);
            
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse()
            {
                ErrorMessages = new List<string>{Convert.ToString(e.Message)},
                IsSuccessStatusCode = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
        }
    }
}