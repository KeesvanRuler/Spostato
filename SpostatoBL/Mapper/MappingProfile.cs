using AutoMapper;
using CrimeGameBlazor_DataAccess;
using CrimeGameBlazor_Models;

namespace SpostatoBL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
