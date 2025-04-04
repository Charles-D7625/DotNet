using Microsoft.AspNetCore.Identity;

namespace DotNetApplication.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}