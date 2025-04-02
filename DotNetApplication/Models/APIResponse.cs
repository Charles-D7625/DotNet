using System.Net;

namespace DotNetApplication.Models;

public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; } = true;
    public List<string> ErrorMessages { get; set; }
    public object Result { get; set; }
}