using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Contents;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Sections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService,
                                  ISectionService sectionService,
                                  IContentService contentService,
                                  ControllerValidator validator,
                                  IMapper mapper) : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseRequestDTO request)
        {
            try
            {
                await validator.ValidateEducatorPrevilege(User.Claims, request.EducatorId);

                var course = await courseService.Add(mapper.Map<CourseDTO>(request));

                return Ok(course);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Not having enought permissions"));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Sections")]
        public async Task<ActionResult<SectionDto>> GetSectionsForCourse([FromQuery] int courseId)
        {
            try
            {
                var sections = await sectionService.GetSectionForCourse(courseId);

                return Ok(sections);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<CourseDTO>> GetCourse([FromQuery] int courseId)
        {
            try
            {
                if (courseId == 0)
                    throw new EntityNotFoundException("Course Id is required");

                var course = await courseService.GetById(courseId);

                return Ok(course);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<CourseDTO>> UpdateCourse([FromBody] CourseDTO course)
        {
            try
            {
                await validator.ValidateEducatorPrevilege(User.Claims, course.EducatorId);
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, course.Id);

                var UpdatedCourse = await courseService.Update(course);

                return Ok(UpdatedCourse);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<CourseDTO>> DeleteCourse([FromQuery] int courseId)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, courseId);

                var removedSections = await sectionService.DeleteSectionsForCourse(courseId);

                foreach (var section in removedSections)
                {
                    await contentService.DeleteBySection(section.Id);
                }

                var deletedCourse = await courseService.DeleteById(courseId);

                return Ok(deletedCourse);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Student-Courses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCoursesForStudent()
        {
            try
            {

                var studentId = ControllerValidator.GetUserIdFromClaims(User.Claims);

                var courses = await courseService.GetCoursesForStudent(studentId);

                return Ok(courses);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Educator-Courses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCoursesForEducator([FromQuery] int educatorId)
        {
            try
            {
                var courses = await courseService.GetCoursesForEducator(educatorId);

                return Ok(courses);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
