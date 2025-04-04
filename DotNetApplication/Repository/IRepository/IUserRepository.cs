
using DotNetApplication.Models;
using DotNetApplication.Models.Dto;

namespace DotNetApplication.Repository.IRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
}