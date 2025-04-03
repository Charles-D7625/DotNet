using MagicVilla_MVC.Models;

namespace MagicVilla_MVC.Services.IServices;

public interface IBaseService
{
    APIResponse responseModel { get; set; }
    Task<T> SendAsync<T>(APIRequest apiRequest);
}