using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Notes
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController(INotesService noteService,
                                 IControllerValidator validator,
                                 IMapper mapper) : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        [Authorize]
        public async Task<ActionResult<NoteDto>> CreateNote([FromBody] NoteDto request)
        {
            try
            {
                await validator.ValidateUserPrivilegeForContent(User.Claims, request.ContentId);

                var note = await noteService.Add(mapper.Map<NoteDto>(request));

                return Ok(note);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<NoteDto>> GetNoteById([FromQuery] int noteId)
        {
            try
            {
                var note = await noteService.GetById(noteId);

                await validator.ValidateUserPrivilegeForContent(User.Claims, note.ContentId);

                return Ok(note);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<NoteDto>> UpdateNote([FromBody] NoteDto note)
        {
            try
            {
                await validator.ValidateUserPrivilegeForContent(User.Claims, note.ContentId);

                var updatedNote = await noteService.Update(note);

                return Ok(updatedNote);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<NoteDto>> DeleteNoteById([FromQuery] int noteId)
        {
            try
            {
                var note = await noteService.GetById(noteId);

                await validator.ValidateUserPrivilegeForContent(User.Claims, note.ContentId);

                var deletedNote = await noteService.DeleteById(noteId);

                return Ok(deletedNote);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        //[HttpGet("Student-Notes")]
        //public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesForStudent([FromQuery] int studentId)
        //{
        //    try
        //    {
        //        //await validator.ValidateStudentPrivilege(User.Claims, studentId);

        //        var notes = await noteService.GetNotesForStudent(studentId);

        //        return Ok(notes);
        //    }
        //    catch (UnAuthorisedUserExeception ex)
        //    {
        //        return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpGet("Content-Notes")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesForContent([FromQuery] int contentId)
        {
            try
            {
                var user = validator.GetUserIdFromClaims(User.Claims);

                await validator.ValidateUserPrivilegeForContent(User.Claims, contentId);

                var notes = await noteService.GetNotesForContent(contentId) ?? await noteService.Add(new NoteDto()
                {
                    ContentId = contentId,
                    NoteContent = "",
                    UserId = user
                });

                return Ok(notes);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
