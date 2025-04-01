using DotNetApplication.Models.Dto;

namespace DotNetApplication.Data;

public static class VillaStore
{
    public static List<VillaDTO> villasList = new List<VillaDTO>()
    {
        new VillaDTO { Id = 1, Name = "Pool View", Occupancy = 4, Sqft = 100},
        new VillaDTO { Id = 2, Name = "Moutense View", Occupancy = 2, Sqft = 200 }
    };
}