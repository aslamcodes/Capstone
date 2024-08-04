using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Sections;

namespace EduQuest.Features.Courses

{
    public class CourseService(ICourseRepo courseRepo, IRepository<int, User> userRepo, IRepository<int, StudentCourse> studentCourse, ISectionService sectionService, IMapper mapper) : BaseService<Course, CourseDTO>(courseRepo, mapper), ICourseService
    {
        public override async Task<CourseDTO> Add(CourseDTO entity)
        {
            var course = mapper.Map<Course>(entity);

            course.CourseStatus = CourseStatusEnum.Draft;

            var courseDb = await courseRepo.Add(course);

            return mapper.Map<CourseDTO>(courseDb);
        }
        public async Task<CourseDTO> EnrollStudentIntoCourse(int studentId, int courseId)
        {
            var course = await courseRepo.GetByKey(courseId);

            await studentCourse.Add(new StudentCourse { StudentId = studentId, CourseId = courseId });

            return mapper.Map<CourseDTO>(course);
        }

        public async Task<List<CourseDTO>> GetCoursesForEducator(int educatorId)
        {
            var courses = await courseRepo.GetAll();

            return courses.Where(c => c.EducatorId == educatorId).Select(mapper.Map<CourseDTO>).ToList();
        }

        public async Task<List<CourseDTO>> GetCoursesForStudent(int studentId)
        {
            var courses = (await studentCourse.GetAll()).Where(sc => sc.StudentId == studentId);

            return courses.Select(c => mapper.Map<CourseDTO>(c.Course)).ToList();
        }

        public async Task<ValidityResponseDto> GetCourseValidity(int courseId)
        {
            var messages = new List<ValidityCriteria>();

            var course = await courseRepo.GetByKey(courseId);

            var sections = await sectionService.GetSectionForCourse(courseId);


            messages.Add(new ValidityCriteria { Criteria = "Course should have atleast 4 sections", IsPassed = sections.Count >= 4 });
            messages.Add(new ValidityCriteria { Criteria = "Course should have a price", IsPassed = course.Price != null });
            messages.Add(new ValidityCriteria { Criteria = "Course should have a description, atleast 200 letters", IsPassed = !string.IsNullOrEmpty(course.Description) && course.Description.Length > 200 });
            messages.Add(new ValidityCriteria { Criteria = "Course should have a Name", IsPassed = !string.IsNullOrEmpty(course.Name) });
            messages.Add(new ValidityCriteria { Criteria = "Course should have a Category", IsPassed = course.CourseCategoryId != 0 });
            messages.Add(new ValidityCriteria { Criteria = "Course Should have a thumbnail", IsPassed = !string.IsNullOrEmpty(course.CourseThumbnailPicture) });
            messages.Add(new ValidityCriteria {Criteria = "Course objectives should be atleast 4", IsPassed = !string.IsNullOrEmpty(course.CourseObjective) && course.CourseObjective.Split("|").ToList().Count >= 4});
            messages.Add(new ValidityCriteria {Criteria = "Course should atleast have 1 prerequisite", IsPassed = !string.IsNullOrEmpty(course.Prerequisites) && course.Prerequisites.Split("|").ToList().Count != 0 });
            messages.Add(new ValidityCriteria {Criteria = "Course should have atleast 1 target audience", IsPassed = !string.IsNullOrEmpty(course.TargetAudience) && course.TargetAudience.Split("|").ToList().Count != 0 });

            var response = new ValidityResponseDto()
            {
                Criterias = messages,
                IsValid = messages.All(m => m.IsPassed)
            };


            return response;
        }

        public async Task<List<CourseDTO>> GetCoursesByStatus(CourseStatusEnum courseStatus)
        {
            List<Course> courses = await courseRepo.GetByStatus(courseStatus);

            return courses.ConvertAll(mapper.Map<CourseDTO>);
        }

        public async Task<List<CourseDTO>> SearchCourse(string query)
        {
            var courses = await courseRepo.GetBySearch(query);

            return courses.ConvertAll(mapper.Map<CourseDTO>);
        }

        public async Task<CourseDTO> SetCourseLive(int courseId)
        {
            var course = await courseRepo.GetByKey(courseId);

            course.CourseStatus = CourseStatusEnum.Live;

            var updatedCourse = await courseRepo.Update(course);

            return mapper.Map<CourseDTO>(updatedCourse);
        }

        public async Task<CourseDTO> SetCourseUnderReview(int courseId)
        {
            var course = await courseRepo.GetByKey(courseId);

            if (course.CourseStatus == CourseStatusEnum.Live)
            {
                throw new InvalidCourseStatusException("Course is already live");
            }

            if (course.CourseStatus == CourseStatusEnum.Archived)
            {
                throw new InvalidCourseStatusException("Course is already Archived, Make it a draft");
            }

            course.CourseStatus = CourseStatusEnum.Review;

            var updatedCourse = await courseRepo.Update(course);

            return mapper.Map<CourseDTO>(updatedCourse);
        }

        public async Task<CourseDTO> SetCourseOutdated(int courseId)
        {
            var course = await courseRepo.GetByKey(courseId);

            course.CourseStatus = CourseStatusEnum.Outdated;

            var updatedCourse = await courseRepo.Update(course);

            return mapper.Map<CourseDTO>(updatedCourse);
        }

        public async Task<CourseDTO> SetCourseProfile(int courseId, string fileUrl)
        {
            var course =await courseRepo.GetByKey(courseId);
            
            course.CourseThumbnailPicture = fileUrl;
            
            var updatedCourse= await courseRepo.Update(course);

            return mapper.Map<CourseDTO>(updatedCourse);
        }
    }
}
