using AutoMapper;
using NationalParkAPI_1033.Models;
using NationalParkAPI_1033.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NationalParkApi1033.DTOMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();

        }
    }
}