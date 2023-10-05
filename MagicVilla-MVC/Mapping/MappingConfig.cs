using AutoMapper;
using MagicVilla_MVC.Models.DTO;
using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, CreateDTO>().ReverseMap();
            CreateMap<VillaDTO, UpdateDTO>().ReverseMap();
        
            CreateMap<VillaNumberDTO,CreateNumberDTO>().ReverseMap();
            CreateMap<VillaNumberDTO,UpdateNumberDTO>().ReverseMap();
        }
    }
}
