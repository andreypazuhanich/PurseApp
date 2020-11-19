using AutoMapper;
using PurseApp.Models;
using PurseApp.Models.Dto;

namespace PurseApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForPath(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ForPath(d => d.Password, opt => opt.MapFrom(s => s.Password))
                .ForPath(d => d.UserName, opt => opt.MapFrom(s => s.UserName));
            CreateMap<AuthenticateRequest, User>(MemberList.Destination);

        }
    }
}