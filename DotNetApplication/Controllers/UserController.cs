using System.Net;
using DotNetApplication.Models;
using DotNetApplication.Models.Dto;
using DotNetApplication.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApplication.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    protected APIResponse _response;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        this._response = new APIResponse();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        var loginResponseDTO = await _userRepository.Login(loginRequestDTO);
        if (loginResponseDTO.User == null || string.IsNullOrEmpty(loginResponseDTO.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages.Add("Username or password is incorrect");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccessStatusCode = true;
        _response.Result = loginResponseDTO;
        return Ok(_response);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
    {
        bool ifUserNameUnique = _userRepository.IsUniqueUser(registrationRequestDTO.UserName);
        if (!ifUserNameUnique)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages.Add("Username is already exists");
            return BadRequest(_response);
        }
        
        var user = await _userRepository.Register(registrationRequestDTO);
        if (user == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages.Add("Error while registering");
            return BadRequest(_response);
        }
        
        return Ok(_response);
    }
}