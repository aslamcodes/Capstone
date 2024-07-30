using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Articles;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.CourseCategories;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Notes;
using EduQuest.Features.Orders;
using EduQuest.Features.Sections;
using EduQuest.Features.Videos;

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

            CreateMap<Video, VideoDto>();
            CreateMap<VideoDto, Video>();

            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();

            CreateMap<CourseCategory, CourseCategoryDto>();
            CreateMap<CourseCategoryDto, CourseCategory>();

            CreateMap<Note, NoteDto>();
            CreateMap<NoteDto, Note>();
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

