using MagicVilla_MVC.Models;
using MagicVilla_MVC.Models.Dto;
using MagicVilla_MVC.Services.IServices;
using MagicVilla_Utility;

namespace MagicVilla_MVC.Services;

public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;
    
    public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
    }

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + "/api/villaAPI"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + "/api/villaAPI/" + id
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/villaAPI"
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/villaAPI/" + dto.Id
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = villaUrl + "/api/villaAPI/" + id
        });
    }
}