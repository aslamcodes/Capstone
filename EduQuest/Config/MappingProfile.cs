using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Orders;
using EduQuest.Features.Sections;

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
            CreateMap<CourseRequestDTO, CourseDTO>();

            CreateMap<Content, ContentDto>();
            CreateMap<ContentDto, Content>();

            CreateMap<SectionDto, Section>();
            CreateMap<Section, SectionDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

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

