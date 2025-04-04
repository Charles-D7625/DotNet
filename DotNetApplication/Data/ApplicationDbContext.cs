using DotNetApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetApplication.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {
    }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Villa>().HasData(
            new Villa
            {
                Id = 1, 
                Name = "Villa1",
                Details = "Cool villa",
                ImageUrl = "",
                Occupancy = 5,
                Rate = 200,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now,
            },
            new Villa
            {
                Id = 2, 
                Name = "Villa2",
                Details = "Coolest villa",
                ImageUrl = "",
                Occupancy = 5,
                Rate = 200,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
            new Villa
            {
                Id = 3, 
                Name = "Villa3",
                Details = "Damn cool villa",
                ImageUrl = "",
                Occupancy = 2,
                Rate = 200,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
            new Villa
            {
                Id = 4, 
                Name = "Villa4",
                Details = "So cool villa",
                ImageUrl = "",
                Occupancy = 3,
                Rate = 770,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
            new Villa
            {
                Id = 5, 
                Name = "Villa5",
                Details = "Big villa",
                ImageUrl = "",
                Occupancy = 8,
                Rate = 550,
                Sqft = 850,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
            new Villa
            {
                Id = 6, 
                Name = "Villa6",
                Details = "Very big villa",
                ImageUrl = "",
                Occupancy = 10,
                Rate = 600,
                Sqft = 1000,
                Amenity = "",
                CreatedDate = DateTime.Now
            }
            );
    }
}