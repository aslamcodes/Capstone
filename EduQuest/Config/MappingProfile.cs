using AutoMapper;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Content;
using EduQuest.Features.Content.Dto;
using EduQuest.Features.Course;
using EduQuest.Features.Course.Dto;
using EduQuest.Features.User;

namespace EduQuest.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthResponseDto>().ForMember(res => res.Token, opt => opt.Ignore());

            CreateMap<CourseDTO, Course>()
                .ForMember(d => d.Level, opt => opt.MapFrom((s) => MapLevel(s.Level)));

            CreateMap<Course, CourseDTO>();

            CreateMap<Content, ContentResponseDto>();

        }
        public static CourseLevelEnum MapLevel(string level)
        {
            return level switch
            {
                "Beginner" => CourseLevelEnum.Beginner,
                "Intermediate" => CourseLevelEnum.Intermediate,
                "Advanced" => CourseLevelEnum.Advanced,
                _ => CourseLevelEnum.Beginner,
            };
        }
    }

}

