using AutoMapper;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.User;

namespace EduQuest.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthResponseDto>().ForMember(res => res.Token, opt => opt.Ignore());
        }
    }
}
