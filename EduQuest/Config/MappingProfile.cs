using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Answers;
using EduQuest.Features.Articles;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.CourseCategories;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Notes;
using EduQuest.Features.Orders;
using EduQuest.Features.Questions;
using EduQuest.Features.Reviews;
using EduQuest.Features.Sections;
using EduQuest.Features.Users;
using EduQuest.Features.Videos;

namespace EduQuest.Config
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthResponseDto>().ForMember(res => res.Token, opt => opt.Ignore());
            CreateMap<User, UserProfileDto>();
            CreateMap<UserProfileDto, User>();
            CreateMap<UserProfileDto, EducatorProfileDto>();
            CreateMap<UserProfileUpdateDto, UserProfileDto>();

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
            CreateMap<VideoRequestDto, VideoDto>();

            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();

            CreateMap<CourseCategory, CourseCategoryDto>();
            CreateMap<CourseCategoryDto, CourseCategory>();

            CreateMap<Note, NoteDto>();
            CreateMap<NoteDto, Note>();

            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionRequestDto, QuestionDto>();

            CreateMap<Answer, AnswerDto>();
            CreateMap<AnswerDto, Answer>();
            CreateMap<AnswerRequestDto, AnswerDto>();

            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewRequestDto, ReviewDto>();
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

