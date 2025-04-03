using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetApplication.Data;
using DotNetApplication.Models;
using DotNetApplication.Models.Dto;
using DotNetApplication.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace DotNetApplication.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private string secretKey;

    public UserRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }
    
    public bool IsUniqueUser(string username)
    {
        var user = _context.LocalUsers.FirstOrDefault(x => x.Username == username);
        if (user == null)
        {
            return true;
        }
        return false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = _context.LocalUsers.FirstOrDefault(u => u.Username.ToLower() == loginRequestDTO.UserName.ToLower()
                                                           && u.Password == loginRequestDTO.Password);
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDesctiptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDesctiptor);
        
        LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = user
        };
        
        return loginResponseDTO;
    }

    public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
    {
        LocalUser localUser = new LocalUser()
        {
            Username = registrationRequestDTO.UserName,
            Password = registrationRequestDTO.Password,
            Name = registrationRequestDTO.UserName,
            Role = registrationRequestDTO.Role
        };
        
        _context.LocalUsers.Add(localUser);
        await _context.SaveChangesAsync();
        localUser.Password = "";
        return localUser;
    }
}