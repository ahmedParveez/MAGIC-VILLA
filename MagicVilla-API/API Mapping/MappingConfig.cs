using AutoMapper;
using MagicVilla_API.Models.DTO;
using MagicVilla_API.Models.DTOs.Villa_Number_DTOs;
using MagicVilla_API.Models.Entities;

namespace MagicVilla_VillaAPI.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDTO>().ReverseMap();

            CreateMap<UpdateDTO, VillaDTO>().ReverseMap();
            CreateMap<CreateDTO,VillaDTO>().ReverseMap();

            CreateMap<UpdateDTO, Villa>().ReverseMap();
            CreateMap<CreateDTO, Villa>().ReverseMap();

            // =========================================================
            
            CreateMap<VillaNumber,VillaNumberDTO>().ReverseMap();

            CreateMap<CreateNumberDTO,VillaNumberDTO>().ReverseMap();
            CreateMap<UpdateNumberDTO,VillaNumberDTO>().ReverseMap();

            CreateMap<CreateNumberDTO, VillaNumber>().ReverseMap();
            CreateMap<UpdateNumberDTO, VillaNumber>().ReverseMap();
        }
    }
}
