using AutoMapper;
using DotNetApplication.Models;
using DotNetApplication.Models.Dto;

namespace DotNetApplication;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        //Villa
        CreateMap<Villa, VillaDTO>();
        CreateMap<VillaDTO, Villa>();
        
        CreateMap<VillaCreateDTO, Villa>().ReverseMap();
        CreateMap<VillaUpdateDTO, Villa>().ReverseMap();
        
        //VillaNumber
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
    }
}