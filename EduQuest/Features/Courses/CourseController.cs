﻿using AutoMapper;
using Azure.Storage.Blobs;
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
                                  BlobServiceClient blobService,
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
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
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
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get-Validity")]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ValidityResponseDto>> GetCourseValidity([FromQuery] int courseId)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, courseId);

                var validity = await courseService.GetCourseValidity(courseId);

                return Ok(validity);
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


        [HttpPut("Submit-For-Review")]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<CourseDTO>> SetCourseUnderReview([FromQuery] int courseId)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, courseId);

                var validity = await courseService.GetCourseValidity(courseId);

                if (!validity.IsValid) return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, "Course not passed validity check"));

                var course = await courseService.SetCourseUnderReview(courseId);

                return Ok(course);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (InvalidCourseStatusException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
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

        [HttpPut("Course-Thumbnail")]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<CourseDTO>> SetCourseThumbnail([FromQuery] int courseId, [FromForm] IFormFile thumbnail)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, courseId);

                BlobContainerClient profileContainer = blobService.GetBlobContainerClient("course-images");

                BlobClient blob = profileContainer.GetBlobClient($"{courseId}-profile.jpg");

                if (await blob.ExistsAsync())
                {
                    await blob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    await thumbnail.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await blob.UploadAsync(memoryStream);
                }

                var fileUrl = blob.Uri.AbsoluteUri;

                var course = await courseService.GetById(courseId);

                course.CourseThumbnailPicture = fileUrl;

                var updatedCourse = await courseService.Update(course);

                return Ok(updatedCourse);
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
        [HttpGet("search")]
        public async Task<ActionResult<List<CourseDTO>>> SearchCourse([FromQuery] string query)
        {
            try
            {
                var courses = await courseService.SearchCourse(query);

                return Ok(courses);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("courses-by-status")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<List<CourseDTO>>> GetCoursesByStatus([FromQuery] CourseStatusEnum status)
        {
            try
            {
                var courses = await courseService.GetCoursesByStatus(status);

                return Ok(courses);
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


        [HttpPut("set-course-live")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CourseDTO>> SetCourseLive([FromQuery] int courseId)
        {
            try
            {
                var course = await courseService.SetCourseLive(courseId);

                return Ok(course);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("set-course-outdated")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CourseDTO>> SetCourseOutdated([FromQuery] int courseId)
        {
            try
            {
                var course = await courseService.SetCourseOutdated(courseId);

                return Ok(course);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
